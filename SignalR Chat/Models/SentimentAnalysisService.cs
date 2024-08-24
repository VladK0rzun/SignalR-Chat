using Azure.AI.TextAnalytics;
using Azure;

namespace SignalR_Chat.Models
{
    public class SentimentAnalysisService
    {
        private readonly TextAnalyticsClient _client;

        public SentimentAnalysisService(string endpoint, string apiKey)
        {
            var credentials = new AzureKeyCredential(apiKey);
            _client = new TextAnalyticsClient(new Uri(endpoint), credentials);
        }

        public async Task<DocumentSentiment> AnalyzeSentimentAsync(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentException("Message cannot be null or whitespace.", nameof(message));
            }

                var response = await _client.AnalyzeSentimentAsync(message);
                return response.Value;

        }

        public string GetSentimentLabel(DocumentSentiment sentiment)
        {
            switch (sentiment.Sentiment)
            {
                case TextSentiment.Positive:
                    return "Positive";
                case TextSentiment.Neutral:
                    return "Neutral";
                case TextSentiment.Negative:
                    return "Negative";
                default:
                    return "Mixed";
            }
        }
    }

    public class SimpleDocumentSentiment
    {
        public TextSentiment Sentiment { get; set; }
        public SentimentConfidenceScores ConfidenceScores { get; set; }

        public SimpleDocumentSentiment(TextSentiment sentiment, SentimentConfidenceScores scores)
        {
            Sentiment = sentiment;
            ConfidenceScores = scores;
        }
    }
}
