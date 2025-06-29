using Smart_Dom.DTOs.Notification;
using Smart_Dom.Interfaces;
using Smart_Dom.Models;

namespace Smart_Dom.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly ILogger<NotificationService> _logger;
        private readonly AppDBContext _appDBContext;

        public NotificationService(INotificationRepository notificationRepository, ILogger<NotificationService> logger, AppDBContext appDBContext)
        {
            _notificationRepository = notificationRepository;
            _logger = logger;
            _appDBContext = appDBContext;
        }

        public async Task CreateNotiAsync(CreateNotificationDTO account)
        {
            _logger.LogInformation("Create Notification");
            if (account == null)
            {
                throw new ArgumentNullException("Cannot be null");
            }
            using var transaction = await _appDBContext.Database.BeginTransactionAsync();
            try
            {               
                var notifycation = new NotificationModel()
                {
                    Title = account.Title,
                    Message = account.Message,
                    UserId = account.UserId,
                    TypeNotify = "bill",
                    IsRead = false,
                    CreatedAt = DateTime.Now,
                };
                await _notificationRepository.CreateAsync(notifycation);
                await transaction.CommitAsync();
            }
            catch (Exception ex) 
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error creating Notification");
                throw;
            }
        }

        public Task DeleteNotiAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<NotificationViewDTO>> GetAllNotificationAsync()
        {
            return await _notificationRepository.GetAllNotificationAsync();
        }

        public async Task<NotificationModel?> GetNotiByIdAsync(int id)
        {
            return await _notificationRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<NotificationModel?>> GetNotiByUserIdAsync(int userId)
        {
            return await _notificationRepository.GetByUserIdAsync(userId);
        }

        public async Task UpdateNotiAsync(NotificationModel account)
        {
            await _notificationRepository.UpdateAsync(account);
        }
    }
}
