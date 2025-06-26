using Microsoft.EntityFrameworkCore;
using Smart_Dom.Interfaces;
using Smart_Dom.Models;

namespace Smart_Dom.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly AppDBContext _context;

        public NotificationRepository( AppDBContext appDBContext)
        {
            _context = appDBContext;
        }

        public async Task CreateAsync(NotificationModel notification)
        {
            await _context.AddAsync(notification);
            await SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existNotification = await _context.Notifications.FindAsync(id);
            if (existNotification != null) 
            {
                _context.Notifications.Remove(existNotification);
                await SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException($"Notification with ID {id} not found.");
            }
        }

        public async Task<IEnumerable<NotificationModel>> GetAllNotificationAsync()
        {
            return await _context.Notifications.ToListAsync();
        }

        public async Task<NotificationModel?> GetByIdAsync(int id)
        {
            return await _context.Notifications.FindAsync(id);
        }

        public async Task<NotificationModel?> GetByUserIdAsync(int userId)
        {
            return await _context.Notifications.FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task UpdateAsync(NotificationModel account)
        {
            _context.Notifications.Update(account);
            await SaveChangesAsync();
        }
    }
}
