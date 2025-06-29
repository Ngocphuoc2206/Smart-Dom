using Smart_Dom.Models;

namespace Smart_Dom.Interfaces
{
    public interface IChatMessageService
    {
        Task<ChatMessageModel> SendMessageAsync(string senderId, string receiverId, string content);
        Task<List<ChatMessageModel>> GetChatAsync(string user1Id, string user2Id);
        Task<int> GetUnreadCountAsync(string receiverId);
        Task MarkAsReadAsync(string receiverId, string senderId);
    }
}
