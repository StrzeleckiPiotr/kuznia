﻿using System;
using System.Collections.Generic;
using System.Text;
using System;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;
using System.Collections.Generic;
using Microsoft.Rest;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;


namespace kuznia
{
    public class AzurecognityService
    {
        /// <summary>
        /// Container for subscription credentials. Make sure to enter your valid key.
        //string subscriptionKey = "9c10c2ed2ae445ffa7a8561ed860f4ef"; //Insert your Text Anaytics subscription key
        /// </summary>


        public AzurecognityService()
        {
        

                // Create a client.
                ITextAnalyticsClient client = new TextAnalyticsClient(new ApiKeyServiceClientCredentials())
                {
                    Endpoint = "https://northeurope.api.cognitive.microsoft.com"
                }; //Replace 'westus' with the correct region for your Text Analytics subscription

                Console.OutputEncoding = System.Text.Encoding.UTF8;

                // Extracting language
                Console.WriteLine("===== LANGUAGE EXTRACTION ======");

                var result = client.DetectLanguageAsync(new BatchInput(
                        new List<Input>()
                            {
                          new Input("1", "This is a document written in English."),
                          new Input("2", "Este es un document escrito en Español."),
                          new Input("3", "这是一个用中文写的文件")
                        })).Result;

                // Printing language results.
                foreach (var document in result.Documents)
                {
                    Console.WriteLine("Document ID: {0} , Language: {1}", document.Id, document.DetectedLanguages[0].Name);
                }

                // Getting key-phrases
                Console.WriteLine("\n\n===== KEY-PHRASE EXTRACTION ======");

                KeyPhraseBatchResult result2 = client.KeyPhrasesAsync(new MultiLanguageBatchInput(
                            new List<MultiLanguageInput>()
                            {
                          new MultiLanguageInput("ja", "1", "猫は幸せ"),
                          new MultiLanguageInput("de", "2", "Fahrt nach Stuttgart und dann zum Hotel zu Fu."),
                          new MultiLanguageInput("en", "3", "My cat is stiff as a rock."),
                          new MultiLanguageInput("es", "4", "A mi me encanta el fútbol!")
                            })).Result;

                // Printing keyphrases
                foreach (var document in result2.Documents)
                {
                    Console.WriteLine("Document ID: {0} ", document.Id);

                    Console.WriteLine("\t Key phrases:");

                    foreach (string keyphrase in document.KeyPhrases)
                    {
                        Console.WriteLine("\t\t" + keyphrase);
                    }
                }

                // Extracting sentiment
                Console.WriteLine("\n\n===== SENTIMENT ANALYSIS ======");

                SentimentBatchResult result3 = client.SentimentAsync(
                        new MultiLanguageBatchInput(
                            new List<MultiLanguageInput>()
                            {
                          new MultiLanguageInput("en", "0", "I had the best day of my life."),
                          new MultiLanguageInput("en", "1", "This was a waste of my time. The speaker put me to sleep."),
                          new MultiLanguageInput("es", "2", "No tengo dinero ni nada que dar..."),
                          new MultiLanguageInput("pl", "3", "Piękny hotel, wielkie kocham cie "),
                            })).Result;


                // Printing sentiment results
                foreach (var document in result3.Documents)
                {
                    Console.WriteLine("Document ID: {0} , Sentiment Score: {1:0.00}", document.Id, document.Score);
                }


                
               

                Console.ReadLine();
            }
        public class ApiKeyServiceClientCredentials : ServiceClientCredentials
        {
            public override Task ProcessHttpRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                request.Headers.Add("Ocp-Apim-Subscription-Key", "9c10c2ed2ae445ffa7a8561ed860f4ef");
                return base.ProcessHttpRequestAsync(request, cancellationToken);
            }
        }
    }
    }

