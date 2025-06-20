using Microsoft.AspNetCore.Mvc;
using Smart_Dom.DTOs;
using Smart_Dom.Models;

namespace Smart_Dom.Areas.Room.Controllers
{
    [Area("Room")]
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController: Controller
    {
        private readonly ILogger<RoomController> _logger;
        private readonly AppDBContext _context;
        public RoomController(ILogger<RoomController> logger, AppDBContext context)
        {
            _logger = logger;
            _context = context;
        }
        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogInformation("Retrieving all rooms");
            var rooms = from r in _context.Rooms
                        join h in _context.RoomHistories on r.ID equals h.RoomId into roomHistories
                        from h in roomHistories.DefaultIfEmpty()
                        join u in _context.Users on h.UserId equals u.ID into users
                        from u in users.DefaultIfEmpty()
                        select new
                        {
                            r.ID,
                            r.RoomNumber,
                            r.Floor,
                            r.Area,
                            r.Price,
                            FullName = u != null ? u.FullName : null,
                            r.Description,
                            r.Status,
                            StartDate = h != null ? h.StartDate : (DateTime?)null,
                            EndDate = h != null ? h.EndDate : (DateTime?)null
                        };

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
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var newRoom = new RoomModel()
                {
                    RoomNumber = room.Number,
                    Floor = room.Floor, // Assuming a default floor for simplicity
                    Area = (int)room.Area,
                    Price = (int)room.Price,
                    Description = room.Description,
                    Status = room.Status,
                    Amenities = string.Join(",", room.Amenities ?? new List<string>())
                };
                await _context.Rooms.AddAsync(newRoom);
                await _context.SaveChangesAsync();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating room");
                transaction.Rollback();
                return StatusCode(500, "Internal server error");
            }
            return Ok();
        }
    }
}
