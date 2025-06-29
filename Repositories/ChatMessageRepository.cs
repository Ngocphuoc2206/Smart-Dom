using Microsoft.EntityFrameworkCore;
using Smart_Dom.Interfaces;
using Smart_Dom.Models;

namespace Smart_Dom.Repositories
{
    public class ChatMessageRepository : IChatMessageRepository
    {
        private readonly AppDBContext _context;
        public ChatMessageRepository(AppDBContext dbContext)
        {
            _context = dbContext;
        }
        public async Task<ChatMessageModel> AddMessageAsync(ChatMessageModel message)
        {
            _context.ChatMessages.Add(message);
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task<List<ChatMessageModel>> GetMessagesAsync(string user1Id, string user2Id)
        {
            return await _context.ChatMessages
            .Where(m =>
                (m.SenderId == user1Id && m.ReceiverId == user2Id) ||
                (m.SenderId == user2Id && m.ReceiverId == user1Id))
            .OrderBy(m => m.SentAt)
            .ToListAsync();
        }

        public async Task<int> GetUnreadCountAsync(string receiverId)
        {
            return await _context.ChatMessages
            .CountAsync(m => m.ReceiverId == receiverId && !m.IsRead);
        }

        public async Task MarkMessagesAsReadAsync(string receiverId, string senderId)
        {
            var unreadMessages = await _context.ChatMessages
            .Where(m => m.ReceiverId == receiverId && m.SenderId == senderId && !m.IsRead)
            .ToListAsync();

            foreach (var msg in unreadMessages)
            {
                msg.IsRead = true;
            }

            await _context.SaveChangesAsync();
        }
    }
}
