using System;
using System.Collections.Generic;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;
using Microsoft.Rest;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using kuznia.Interface;

namespace kuznia
{
    public class AzurecognityService : IAzurecognityService
    {
        public string TextDetect(ITextAnalyticsClient client, string report)
        {
            var result = client.DetectLanguageAsync(new BatchInput(
                    new List<Input>()
                        {
                          new Input("1", report)
                    })).Result;


            return result.Documents[0].DetectedLanguages[0].Name;
        }

        public IList<string> KeyPhrases(ITextAnalyticsClient client, string identity, string text)
        {
            KeyPhraseBatchResult result = client.KeyPhrasesAsync(new MultiLanguageBatchInput(
                        new List<MultiLanguageInput>()
                        {
                          new MultiLanguageInput(identity, "1", text)
                            })).Result;

            return result.Documents[0].KeyPhrases;
        }

        public double? SentimentText(ITextAnalyticsClient client, string identity, string text)
        {
            SentimentBatchResult result = client.SentimentAsync(
                        new MultiLanguageBatchInput(
                            new List<MultiLanguageInput>()
                            {

                          new MultiLanguageInput(identity, "1",text)
                            })).Result;

            return result.Documents[0].Score;
        }
        public ITextAnalyticsClient TextAnalyticsClient()
        {
            ITextAnalyticsClient client = new TextAnalyticsClient(new ApiKeyServiceClientCredentials())
            {
                Endpoint = "https://northeurope.api.cognitive.microsoft.com"
            };

            return client;
        }
    }
    public class ApiKeyServiceClientCredentials : ServiceClientCredentials
    {
        public override Task ProcessHttpRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Add("Ocp-Apim-Subscription-Key", "47473c09f5b748788531fd47529e06b4");
            return base.ProcessHttpRequestAsync(request, cancellationToken);
        }
    }
}

