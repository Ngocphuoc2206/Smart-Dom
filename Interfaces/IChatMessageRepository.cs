using Smart_Dom.Models;

namespace Smart_Dom.Interfaces
{
    public interface IChatMessageRepository
    {
        Task<ChatMessageModel> AddMessageAsync(ChatMessageModel message);
        Task<List<ChatMessageModel>> GetMessagesAsync(string user1Id, string user2Id);
        Task<int> GetUnreadCountAsync(string receiverId);
        Task MarkMessagesAsReadAsync(string receiverId, string senderId);
    }
}
