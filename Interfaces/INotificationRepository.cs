using Smart_Dom.DTOs.User;
using Smart_Dom.Models;

namespace Smart_Dom.Interfaces
{
    public interface INotificationRepository
    {
        Task<IEnumerable<NotificationModel>> GetAllNotificationAsync();
        Task<NotificationModel?> GetByIdAsync(int id);
        Task<NotificationModel?> GetByUserIdAsync(int userId);
        Task CreateAsync(NotificationModel account);
        Task DeleteAsync(int id);
        Task UpdateAsync(NotificationModel account);
        Task<bool> SaveChangesAsync();
    }
}
