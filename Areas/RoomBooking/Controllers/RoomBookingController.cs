using Microsoft.AspNetCore.Mvc;
using Smart_Dom.DTOs.RoomBooking;
using Smart_Dom.Services;
namespace Smart_Dom.Areas.RoomBooking.Controllers
{
    [Area("RoomBooking")]
    [Route("api/[controller]")]
    [ApiController]
    public class RoomBookingController : Controller
    {
        private readonly ILogger<RoomBookingController> _logger;
        private readonly IRoomBookingService _roomBookingService;
        private readonly IRoomService _roomService;
        public RoomBookingController(ILogger<RoomBookingController> logger, IRoomBookingService roomBookingService, IRoomService roomService)
        {
            _logger = logger;
            _roomBookingService = roomBookingService;
            _roomService = roomService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("Retrieving all room bookings");
            var bookings = await _roomBookingService.GetAllBookingsAsync();
            return Ok(bookings);
        }

        [HttpGet("tenant")]
        public async Task<IActionResult> GetAllTenant()
        {
            _logger.LogInformation("Retrieving all room bookings with tenant information");
            var booking = await _roomBookingService.GetRoomAllInformation();
            if (booking == null)
            {
                _logger.LogWarning("No bookings found with tenant information");
                return NotFound("No bookings found with tenant information");
            }
            return Ok(booking);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            _logger.LogInformation($"Retrieving booking with ID {id}");
            var booking = await _roomBookingService.GetBookingByIdAsync(id);
            if (booking == null)
            {
                _logger.LogWarning($"Booking with ID {id} not found");
                return NotFound($"Booking with ID {id} not found");
            }
            return Ok(booking);
        }

        [HttpGet("expired/{id}")]
        public async Task<IActionResult> Update(int id)
        {
            _logger.LogInformation($"Updating booking with ID {id}");
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for room booking update");
                return BadRequest(ModelState);
            }
            var existingBooking = await _roomBookingService.GetBookingByIdAsync(id);
            if (existingBooking == null)
            {
                _logger.LogWarning($"Booking with ID {id} not found for update");
                return NotFound($"Booking with ID {id} not found");
            }
            var room = await _roomService.GetRoomByIdAsync(existingBooking.RoomId);
            if (room == null)
            {
                _logger.LogWarning($"Room with ID {existingBooking.RoomId} not found for booking update");
                return NotFound($"Room with ID {existingBooking.RoomId} not found");
            }
            existingBooking.Status = "overdue"; // Ensure the ID is set correctly
            room.Status = "inactive"; // Update room status to inactive
            await _roomBookingService.UpdateBookingAsync(existingBooking);
            await _roomService.UpdateRoom(room);
            return Ok();
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateRoomBookingDTO booking)
        {
            _logger.LogInformation("Creating a new room booking");
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for room booking creation");
                return BadRequest(ModelState);
            }
            await _roomBookingService.AddBookingAsync(booking);
            return Ok();
        }

        [HttpPut("confirm/{id}")]
        public async Task<IActionResult> Confirm(int id)
        {
            _logger.LogInformation($"Confirming booking with ID {id}");
            var booking = await _roomBookingService.GetBookingByIdAsync(id);
            if (booking == null)
            {
                _logger.LogWarning($"Booking with ID {id} not found");
                return NotFound($"Booking with ID {id} not found");
            }
            var room = await _roomService.GetRoomByIdAsync(booking.RoomId);
            if (room == null)
            {
                _logger.LogWarning($"Room with ID {booking.RoomId} not found for booking confirmation");
                return NotFound($"Room with ID {booking.RoomId} not found");
            }
            booking.Status = "occupied";
            room.Status = "occupied";
            await _roomBookingService.UpdateBookingAsync(booking);
            await _roomService.UpdateRoom(room);
            return Ok();
        }

        
    }
}
