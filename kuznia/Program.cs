using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.ML;
using Microsoft.ML.Core.Data;
using Microsoft.ML.Runtime.Api;
using Microsoft.ML.Runtime.Data;
using Microsoft.ML.Transforms.Text;
namespace kuznia
{
    class Program
    {
        static readonly string _trainDataPath = Path.Combine(Environment.CurrentDirectory, "Data", "wikipedia-detox-250-line-data.tsv");
        static readonly string _testDataPath = Path.Combine(Environment.CurrentDirectory, "Data", "wikipedia-detox-250-line-test.tsv");
        static readonly string _modelPath = Path.Combine(Environment.CurrentDirectory, "Data", "Model.zip");
        static TextLoader _textLoader;
        MLContext mlConpext;

        static void Main(string[] args)
        {
            AzurecognityService azurecognityService = new AzurecognityService();
            
            MLContext mlContext = new MLContext(seed: 0);
            _textLoader = mlContext.Data.TextReader(new TextLoader.Arguments()
            {
                Separator = "tab",
                HasHeader = true,
                Column = new[]
                                                                {
                                                                  new TextLoader.Column("Label", DataKind.Bool, 0),
                                                                  new TextLoader.Column("SentimentText", DataKind.Text, 1)
                                                                }
            }
            );
            var model = Train(mlContext, _trainDataPath);
            // </Snippet5>

            // <Snippet11>
            Evaluate(mlContext, model);
            // </Snippet11>

            // <Snippet16>
            Predict(mlContext, model);
            // </Snippet16>

            // <Snippet25>
            PredictWithModelLoadedFromFile(mlContext);
            // </Snippet25>


            Console.WriteLine();
            Console.WriteLine("=============== End of process ===============");
            Thread.Sleep(50000000);


        }

    

        public static ITransformer Train(MLContext mlContext, string dataPath)
        {
            
            IDataView dataView = _textLoader.Read(dataPath);
            
            var pipeline = mlContext.Transforms.Text.FeaturizeText("SentimentText", "Features").Append(mlContext.BinaryClassification.Trainers.FastTree(numLeaves: 50, numTrees: 50, minDatapointsInLeafs: 20));
            
            Console.WriteLine("=============== Create and Train the Model ===============");
            var model = pipeline.Fit(dataView);
            Console.WriteLine("=============== End of training ===============");
            Console.WriteLine();
            // </Snippet9>

            // Returns the model we trained to use for evaluation.
            // <Snippet10>
            return model;
            // </Snippet10>
        }

        public static void Evaluate(MLContext mlContext, ITransformer model)
        {
            
            IDataView dataView = _textLoader.Read(_testDataPath);
          
            Console.WriteLine("=============== Evaluating Model accuracy with Test data===============");
            var predictions = model.Transform(dataView);
           
            var metrics = mlContext.BinaryClassification.Evaluate(predictions, "Label");
          
            Console.WriteLine();
            Console.WriteLine("Model quality metrics evaluation");
            Console.WriteLine("--------------------------------");
            Console.WriteLine($"Accuracy: {metrics.Accuracy:P2}");
            Console.WriteLine($"Auc: {metrics.Auc:P2}");
            Console.WriteLine($"F1Score: {metrics.F1Score:P2}");
            Console.WriteLine("=============== End of model evaluation ===============");
           
            SaveModelAsFile(mlContext, model);
         
        }

        private static void Predict(MLContext mlContext, ITransformer model)
        {
            
            var predictionFunction = model.MakePredictionFunction<SentimentData, SentimentPrediction>(mlContext);
          

            
            SentimentData sampleStatement = new SentimentData
            {
                SentimentText = "This is a very good movie"
            };
          
            var resultprediction = predictionFunction.Predict(sampleStatement);
          
            Console.WriteLine();
            Console.WriteLine("=============== Prediction Test of model with a single sample and test dataset ===============");

            Console.WriteLine();
            Console.WriteLine($"Sentiment: {sampleStatement.SentimentText} | Prediction: {(Convert.ToBoolean(resultprediction.Prediction) ? "Toxic" : "Not Toxic")} | Probability: {resultprediction.Probability} ");
            Console.WriteLine("=============== End of Predictions ===============");
            Console.WriteLine();
           
        }

        public static void PredictWithModelLoadedFromFile(MLContext mlContext)
        {
           
            IEnumerable<SentimentData> sentiments = new[]
            {
                new SentimentData
                {
                    SentimentText = "This is a very rude movie, war "
                },
                new SentimentData
                {
                    SentimentText = "He is the best, and the article should say that."
                }
            };
          
            ITransformer loadedModel;
            using (var stream = new FileStream(_modelPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                loadedModel = mlContext.Model.Load(stream);
            }
          
            var sentimentStreamingDataView = mlContext.CreateStreamingDataView(sentiments);
            var predictions = loadedModel.Transform(sentimentStreamingDataView);

            
            var predictedResults = predictions.AsEnumerable<SentimentPrediction>(mlContext, reuseRowObject: false);

            Console.WriteLine();

            Console.WriteLine("=============== Prediction Test of loaded model with a multiple samples ===============");
        

            Console.WriteLine();

           
            var sentimentsAndPredictions = sentiments.Zip(predictedResults, (sentiment, prediction) => (sentiment, prediction));

            foreach (var item in sentimentsAndPredictions)
            {
                Console.WriteLine($"Sentiment: {item.sentiment.SentimentText} | Prediction: {(Convert.ToBoolean(item.prediction.Prediction) ? "Toxic" : "Not Toxic")} | Probability: {item.prediction.Probability} ");
            }
            Console.WriteLine("=============== End of predictions ===============");
     
        }

        // Saves the model we trained to a zip file.

        private static void SaveModelAsFile(MLContext mlContext, ITransformer model)
        {
            // <Snippet24> 
            using (var fs = new FileStream(_modelPath, FileMode.Create, FileAccess.Write, FileShare.Write))
                mlContext.Model.Save(model, fs);
            // </Snippet24>

            Console.WriteLine("The model is saved to {0}", _modelPath);
        }

    }
}
