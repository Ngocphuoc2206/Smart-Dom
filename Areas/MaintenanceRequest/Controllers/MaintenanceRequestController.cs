using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Smart_Dom.DTOs.MaintenanceRequest;
using Smart_Dom.DTOs.Notification;
using Smart_Dom.Models;
using Smart_Dom.Services;
using System.Threading.Tasks;

namespace Smart_Dom.Areas.MaintenanceRequest.Controllers
{
    [Area("MaintenanceRequest")]
    [Route("api/[controller]")]
    [ApiController]
    public class MaintenanceRequestController: Controller
    {
        private readonly IMaintenanceRequestService _maintenanceRequestService;
        private readonly ILogger<MaintenanceRequestController> _logger;
        private readonly IRoomService _roomService;
        private readonly INotificationService _notificationService;
        private readonly IEmailService _emailService;
        private readonly IUserService _userService;
        public MaintenanceRequestController(IMaintenanceRequestService maintenanceRequestService, 
            ILogger<MaintenanceRequestController> logger, IRoomService roomService, 
            INotificationService notificationService, IEmailService emailService, IUserService userService)
        {
            _maintenanceRequestService = maintenanceRequestService;
            _logger = logger;
            _roomService = roomService;
            _notificationService = notificationService;
            _emailService = emailService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Get() {
            _logger.LogInformation("Fetching Maintenance Request...");
            var requests = await _maintenanceRequestService.GetAllRequestsAsync();
            return Ok(requests);
        }

        [HttpGet("tenant")]
        public async Task<IActionResult> GetInfo()
        {
            _logger.LogInformation("Fetching Maintenance Request...");
            var requests = await _maintenanceRequestService.GetAllInfoRequestsAsync();
            return Ok(requests);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateMaintenanceRequestDTO maintenanceRequest)
        {
            _logger.LogInformation($"Create Maintenance Request...");
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for request creation");
                return BadRequest(ModelState);
            }
            try
            {
                await _maintenanceRequestService.CreateRequestAsync(maintenanceRequest);
                return Ok();
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error occurred while registering account");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("update/status/{id}")]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateStatusRequestDTO updateStatus, int id)
        {
            _logger.LogInformation($"Create Maintenance Request...");
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for request creation");
                return BadRequest(ModelState);
            }
            var existingRequest = await _maintenanceRequestService.GetRequestByIdAsync(id);
            
            if (existingRequest == null)
            {
                _logger.LogError("Cannot find request by ID");
                return StatusCode(500, "Internal server error");
            }
            try
            {
                existingRequest.Status = updateStatus.Status;
                await _maintenanceRequestService.HandleRequestAsync(existingRequest);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while registering account");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("notify/{id}")]
        public async Task<IActionResult> NotifyTenant(int id)
        {
            _logger.LogInformation("Creating Notification and Send Email...");
            var report = await _maintenanceRequestService.GetRequestByIdAsync(id);

            if (report == null)
                return NotFound();
            //Find Room Exist By ID
            var room = await _roomService.GetRoomByIdAsync(report.RoomId);
            if (room == null) return NotFound();

            //Find User Exist By ID
            var user = await _userService.GetUserByIdAsync(report.UserId);
            if (user == null) return NotFound();
            // 1. Lưu Notification
            try
            {
                var notification = new CreateNotificationDTO()
                {
                    UserId = report.UserId, // hoặc report.Tenant.Email
                    Title = "Cập nhật trạng thái báo cáo",
                    Message = $"Báo cáo sự cố của bạn tại phòng {room.RoomNumber} đã được cập nhật: {report.Status}.",
                };
                await _notificationService.CreateNotiAsync(notification);

                // 2. Gửi Email
                await _emailService.SendEmailAsync(
                    user.Email,
                    $"CẬP NHẬT BÁO CÁO SỰ CỐ CỦA PHÒNG {room.RoomNumber}",
                    notification.Message
                );
                return Ok(new { message = "Đã gửi thông báo thành công." });
            }
            catch
            {
                _logger.LogError("Error can't create Notify and SendEmail");
                return StatusCode(500, "Cannot Send Notify");
            }
            
        }


        [HttpPut("update/response/{id}")]
        public async Task<IActionResult> UpdateResponse(int id, [FromBody] UpdateStatusRequestDTO updateResponse)
        {
            _logger.LogInformation($"Create Maintenance Request...");
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for request creation");
                return BadRequest(ModelState);
            }
            _logger.LogInformation($"Update Respone.... ${updateResponse.Response}");
            var existingRequest = await _maintenanceRequestService.GetRequestByIdAsync(id);

            if (existingRequest == null)
            {
                _logger.LogError("Cannot find request by ID");
                return StatusCode(500, "Internal server error");
            }
            try
            {
                existingRequest.ResponeFromOwners = updateResponse.Response;
                await _maintenanceRequestService.HandleRequestAsync(existingRequest);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while registering account");
                return StatusCode(500, "Internal server error");
            }
        }
        
    }
}
