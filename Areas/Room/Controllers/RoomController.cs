using Microsoft.AspNetCore.Mvc;
using Smart_Dom.DTOs;
using Smart_Dom.Models;
using Smart_Dom.Services;

namespace Smart_Dom.Areas.Room.Controllers
{
    [Area("Room")]
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController: Controller
    {
        private readonly ILogger<RoomController> _logger;
        private readonly IRoomService _roomService;
        public RoomController(ILogger<RoomController> logger, IRoomService roomService)
        {
            _logger = logger;
            _roomService = roomService;
        }
        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogInformation("Retrieving all rooms");
            var rooms = _roomService.GetAllRoomsWithHistoryAsync();
            return Ok(rooms);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] RoomDTO room)
        {
            _logger.LogInformation("Creating a new room");
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for room creation");
                return BadRequest(ModelState);
            }
            await _roomService.CreateRoomAsync(room);
            return Ok();
        }
    }
}
