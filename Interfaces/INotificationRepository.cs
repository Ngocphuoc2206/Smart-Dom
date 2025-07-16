using Smart_Dom.DTOs.Notification;
using Smart_Dom.DTOs.User;
using Smart_Dom.Models;

namespace Smart_Dom.Interfaces
{
    public interface INotificationRepository
    {
        Task<IEnumerable<NotificationViewDTO>> GetAllNotificationAsync();
        Task<IEnumerable<NotificationViewDTO>> GetAllNotificationByUserIDAsync(int id);
        Task<NotificationModel?> GetByIdAsync(int id);
        Task<IEnumerable<NotificationModel?>> GetByUserIdAsync(int userId);
        Task CreateAsync(NotificationModel account);
        Task DeleteAsync(int id);
        Task UpdateAsync(NotificationModel account);
        Task<bool> SaveChangesAsync();
    }
}
