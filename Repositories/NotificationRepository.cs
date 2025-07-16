using Microsoft.EntityFrameworkCore;
using Smart_Dom.DTOs.Notification;
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

        public async Task<IEnumerable<NotificationViewDTO>> GetAllNotificationAsync()
        {
            var noties = from notification in _context.Notifications
                         join m in _context.MaintenanceRequests on notification.UserId equals m.UserId
                         select new NotificationViewDTO
                         {
                             Id = notification.Id,
                             Title = notification.Title,
                             TypeNotify = notification.TypeNotify,
                             IsRead = notification.IsRead,
                             Message = notification.Message,
                             Priority = m.PriorityLevel,
                             CreatedAt = notification.CreatedAt,
                         };
            return await noties.ToListAsync();
        }

        public async Task<IEnumerable<NotificationViewDTO>> GetAllNotificationByUserIDAsync(int id)
        {
            var noties = from notification in _context.Notifications
                         join m in _context.MaintenanceRequests on notification.UserId equals m.UserId into mainGroup
                         from maint in mainGroup.DefaultIfEmpty()
                         where notification.UserId == id
                         select new NotificationViewDTO
                         {
                             Id = notification.Id,
                             Title = notification.Title,
                             TypeNotify = notification.TypeNotify,
                             IsRead = notification.IsRead,
                             Message = notification.Message,
                             Priority = maint.PriorityLevel,
                             CreatedAt = notification.CreatedAt,
                         };
            return await noties.ToListAsync();
        }

        public async Task<NotificationModel?> GetByIdAsync(int id)
        {
            return await _context.Notifications.FindAsync(id);
        }

        public async Task<IEnumerable<NotificationModel?>> GetByUserIdAsync(int userId)
        {
            var notifications = await _context.Notifications
                .Where(x => x.UserId == userId)
                .ToListAsync();

            return notifications;
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
