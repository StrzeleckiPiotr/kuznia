
using kuznia.Interface;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics;
using System;
using System.Threading;

namespace kuznia
{
    class Program
    { 
        
        
        static void Main(string[] args)
        {
            while (true)
            {
                System.Console.WriteLine("1.ML.NetTral  - sentiment analysis");
                System.Console.WriteLine("2.Azure - Extracting language ");
                System.Console.WriteLine("3.Azure - Key-phrases ");
                System.Console.WriteLine("4.Azure - sentiment analysis");
                try
                {
                    int i = int.Parse(System.Console.ReadLine());
                    switch (i)
                    {
                        case 0:
                        case 1:
                            {
                                MlNetTrial mlNetTrial = new MlNetTrial();
                                Console.Write("Insert predict text");
                                Console.WriteLine();
                                string inputdata = Console.ReadLine();
                                mlNetTrial.Execute(inputdata);
                                break;
                            }
                        case 2:
                            {
                                Console.Write("Insert text to detect");
                                Console.WriteLine();
                                string inputdata = Console.ReadLine();
                                var azurecognityService = new AzurecognityService();
                                var client = azurecognityService.TextAnalyticsClient();
                                var result = azurecognityService.TextDetect(client, inputdata);
                                Console.Write(result);
                                break;
                            }
                        case 3:
                            {
                                Console.Write("Insert text to get KeyPhrases");
                                Console.WriteLine();
                                string inputdata = Console.ReadLine();
                                var azurecognityService = new AzurecognityService();
                                var client = azurecognityService.TextAnalyticsClient();
                                var identity = azurecognityService.TextDetect(client, inputdata);
                                var result = azurecognityService.KeyPhrases(client, identity, inputdata);
                                Console.Write(result);
                                break;
                            }
                        case 4:
                            {
                                Console.Write("Insert text to TextAnalytics");
                                Console.WriteLine();
                                string inputdata = Console.ReadLine();
                                var azurecognityService = new AzurecognityService();
                                var client = azurecognityService.TextAnalyticsClient();
                                var identity = azurecognityService.TextDetect(client, inputdata);
                                var result = azurecognityService.SentimentText(client, identity, inputdata);
                                Console.Write(result);
                                break;
                            }
                        default:
                            {
                                System.Console.WriteLine("Other number");
                                break;
                            }
                    }
                }
                catch
                {
                    System.Console.WriteLine("Error");
                }
                finally
                {
                    Console.WriteLine();
                    Console.WriteLine("=============== End of process ===============");
                }
            }

            
        }
    }
}
            
