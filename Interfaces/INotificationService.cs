using Smart_Dom.DTOs.Notification;
using Smart_Dom.Models;
using System.Threading.Tasks;

namespace Smart_Dom.Interfaces
{
    public interface INotificationService
    {
        Task<IEnumerable<NotificationModel>> GetAllNotificationAsync();
        Task<NotificationModel?> GetNotiByIdAsync(int id);
        Task<NotificationModel?> GetNotiByUserIdAsync(int userId);
        Task CreateNotiAsync(CreateNotificationDTO account);
        Task DeleteNotiAsync(int id);
        Task UpdateNotiAsync(CreateNotificationDTO account);
    }
}
