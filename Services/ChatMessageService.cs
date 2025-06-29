using Smart_Dom.Interfaces;
using Smart_Dom.Models;

namespace Smart_Dom.Services
{
    public class ChatMessageService : IChatMessageService
    {
        private readonly IChatMessageRepository _repository;
        private readonly ILogger<ChatMessageService> _logger;

        public ChatMessageService(IChatMessageRepository repository, ILogger<ChatMessageService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<ChatMessageModel> SendMessageAsync(string senderId, string receiverId, string content)
        {
            _logger.LogInformation($"Sender ID: {senderId} ReceivedId: {receiverId}");
            var message = new ChatMessageModel
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Content = content,
                SentAt = DateTime.UtcNow,
                IsRead = false
            };

            return await _repository.AddMessageAsync(message);
        }

        public async Task<List<ChatMessageModel>> GetChatAsync(string user1Id, string user2Id)
        {
            return await _repository.GetMessagesAsync(user1Id, user2Id);
        }

        public async Task<int> GetUnreadCountAsync(string receiverId)
        {
            return await _repository.GetUnreadCountAsync(receiverId);
        }

        public async Task MarkAsReadAsync(string receiverId, string senderId)
        {
            await _repository.MarkMessagesAsReadAsync(receiverId, senderId);
        }
    }
}
