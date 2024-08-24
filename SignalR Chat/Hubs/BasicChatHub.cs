using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SignalR_Chat.Data;
using SignalR_Chat.Models;

namespace SignalR_Chat.Hubs
{
    public class BasicChatHub : Hub
    {
        private readonly ApplicationDbContext _db;
        private readonly SentimentAnalysisService _sentimentAnalysisService;

        public BasicChatHub(ApplicationDbContext db, SentimentAnalysisService sentimentAnalysisService)
        {
            _db = db;
            _sentimentAnalysisService = sentimentAnalysisService;
        }

        public async Task SendMessageToAll(string user, string message)
        {
            var sentimentResult = await _sentimentAnalysisService.AnalyzeSentimentAsync(message);
            var sentimentLabel = _sentimentAnalysisService.GetSentimentLabel(sentimentResult);

            var chatMessage = new ChatMessage
            {
                Sender = user,
                Receiver = null,  
                Message = message,
                SentAt = DateTime.UtcNow,
                Sentiment = sentimentLabel
            };

            _db.ChatMessages.Add(chatMessage);
            await _db.SaveChangesAsync();

            await Clients.All.SendAsync("MessageReceived", user, message, sentimentLabel);
        }

        [Authorize]
        public async Task SendMessageToReceiver(string sender, string receiver, string message)
        {
            var userId = _db.Users.FirstOrDefault(u => u.Email.ToLower() == receiver.ToLower())?.Id;

            if (!string.IsNullOrEmpty(userId))
            {
                var sentimentResult = await _sentimentAnalysisService.AnalyzeSentimentAsync(message);
                var sentimentLabel = _sentimentAnalysisService.GetSentimentLabel(sentimentResult);

                var chatMessage = new ChatMessage
                {
                    Sender = sender,
                    Receiver = receiver,
                    Message = message,
                    SentAt = DateTime.UtcNow,
                    Sentiment = sentimentLabel
                };

                _db.ChatMessages.Add(chatMessage);
                await _db.SaveChangesAsync();

                await Clients.User(userId).SendAsync("MessageReceived", sender, message, sentimentLabel);
            }
        }
    }
}
