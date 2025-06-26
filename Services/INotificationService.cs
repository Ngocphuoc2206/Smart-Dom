using Smart_Dom.Models;
using System.Threading.Tasks;

namespace Smart_Dom.Services
{
    public interface INotificationService
    {
        Task<IEnumerable<NotificationModel>> GetAllNotificationAsync();
        Task<NotificationModel?> GetByIdAsync(int id);
        Task<NotificationModel?> GetByUserIdAsync(int userId);
        Task CreateAsync(NotificationModel account);
        Task DeleteAsync(int id);
        Task UpdateAsync(NotificationModel account);
    }
}
