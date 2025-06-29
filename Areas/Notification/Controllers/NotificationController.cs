using Microsoft.AspNetCore.Mvc;
using Smart_Dom.DTOs.Notification;
using Smart_Dom.Interfaces;

namespace Smart_Dom.Areas.Notification.Controllers
{
    [Area("Notification")]
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : Controller
    {
        private readonly INotificationService _notificationService;
        private readonly ILogger<NotificationController> _logger;
        public NotificationController(INotificationService notificationService, ILogger<NotificationController> logger)
        {
            _notificationService = notificationService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("Fetching Notification");
            var noties = await _notificationService.GetAllNotificationAsync();
            return Ok(noties); // Pass 'noties' to the Ok() method to make use of the variable.
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody]CreateNotificationDTO notification)
        {
            _logger.LogInformation($"Creating a new notification with UserID: {notification.UserId}");
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for Notify creation");
                return BadRequest(ModelState);
            }
            await _notificationService.CreateNotiAsync(notification);
            return Ok();
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id)
        {
            _logger.LogInformation($"Update a new notification with UserID: {id}");
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for Notify creation");
                return BadRequest(ModelState);
            }
            var notifications = await _notificationService.GetNotiByUserIdAsync(id);
            foreach (var notif in notifications)
            {
                notif.IsRead = true;
                await _notificationService.UpdateNotiAsync(notif);
            }
            return Ok();
        }
    }
}
