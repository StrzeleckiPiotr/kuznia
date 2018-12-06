using Microsoft.Azure.CognitiveServices.Language.TextAnalytics;
using System.Collections.Generic;


namespace kuznia.Interface
{
    interface IAzurecognityService
    {
        string TextDetect(ITextAnalyticsClient client, string report);

        IList<string> KeyPhrases(ITextAnalyticsClient client, string identity, string text);

        double? SentimentText(ITextAnalyticsClient client, string identity, string text);

        ITextAnalyticsClient TextAnalyticsClient();
    }
}
