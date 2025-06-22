using Microsoft.AspNetCore.Mvc;
using Smart_Dom.DTOs;
using Smart_Dom.Models;
using Smart_Dom.Services;

namespace Smart_Dom.Areas.Room.Controllers
{
    [Area("Room")]
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : Controller
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

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] RoomDTO room)
        {
            _logger.LogInformation($"Updating room with ID {id}");
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for room update");
                return BadRequest(ModelState);
            }
            room.ID = id; // Ensure the ID is set for the update
            await _roomService.UpdateRoomAsync(room);
            return Ok();
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation($"Deleting room with ID {id}");
            try
            {
                await _roomService.DeleteRoomAsync(id);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, $"Room with ID {id} not found for deletion");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting room");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
