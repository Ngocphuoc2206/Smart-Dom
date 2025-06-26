using Smart_Dom.Interfaces;
using Smart_Dom.Models;

namespace Smart_Dom.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(INotificationRepository notificationRepository, ILogger<NotificationService> logger)
        {
            _notificationRepository = notificationRepository;
            _logger = logger;
        }

        public Task CreateNotiAsync(NotificationModel account)
        {
            throw new NotImplementedException();
        }

        public Task DeleteNotiAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<NotificationModel>> GetAllNotificationAsync()
        {
            return await _notificationRepository.GetAllNotificationAsync();
        }

        public async Task<NotificationModel?> GetNotiByIdAsync(int id)
        {
            return await _notificationRepository.GetByIdAsync(id);
        }

        public async Task<NotificationModel?> GetNotiByUserIdAsync(int userId)
        {
            return await _notificationRepository.GetByUserIdAsync(userId);
        }

        public Task UpdateNotiAsync(NotificationModel account)
        {
            throw new NotImplementedException();
        }
    }
}
