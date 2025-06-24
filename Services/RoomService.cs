using Microsoft.EntityFrameworkCore;
using Smart_Dom.DTOs;
using Smart_Dom.Interfaces;
using Smart_Dom.Models;
using Smart_Dom.Repositories;

namespace Smart_Dom.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly AppDBContext _context;
        private readonly ILogger<RoomService> _logger;
        public RoomService(IRoomRepository roomRepository, AppDBContext context, ILogger<RoomService> logger)
        {
            _roomRepository = roomRepository;
            _context = context;
            _logger = logger;
        }
        public async Task CreateRoomAsync(RoomDTO room)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var newRoom = new RoomModel()
                {
                    RoomNumber = room.RoomNumber,
                    Floor = room.Floor, // Assuming a default floor for simplicity
                    Area = (int)room.Area,
                    Price = (int)room.Price,
                    Description = room.Description,
                    Status = room.Status,
                    Amenities = room.Amenities != null ? string.Join(",", room.Amenities) : string.Empty
                };
                await _roomRepository.CreateAsync(newRoom);
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating room");
                transaction.Rollback();
                throw new Exception("Error creating room", ex);
            }
        }

        public async Task DeleteRoomAsync(int id)
        {
            var existingRoom = await _roomRepository.GetByIdAsync(id);
            if (existingRoom == null)
            {
                _logger.LogWarning($"Room with ID {id} not found for deletion.");
                throw new KeyNotFoundException($"Room with ID {id} not found.");
            }
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                await _roomRepository.DeleteAsync(id);
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting room");
                transaction.Rollback();
                throw new Exception("Error deleting room", ex);
            }
        }

        public async Task<IEnumerable<RoomModel>> GetAllRoomAsync()
        {
            return await _roomRepository.GetAllAsync();
        }

        public Task<IEnumerable<RoomDTO>> GetAllRoomsWithHistoryAsync()
        {
            return _roomRepository.GetAllWithHistoryAsync();
        }

        public async Task<RoomModel?> GetRoomByIdAsync(int id)
        {
            return await _roomRepository.GetByIdAsync(id);
        }

        public async Task UpdateRoom(RoomModel room)
        {
            await _roomRepository.UpdateAsync(room);
        }

        public async Task UpdateRoomAsync(RoomDTO room)
        {
            //Check room exists
            var existingRoom = await _roomRepository.GetByIdAsync(room.ID);
            if (existingRoom == null)
            {
                _logger.LogWarning($"Room with ID {room.ID} not found for update.");
                throw new KeyNotFoundException($"Room with ID {room.ID} not found.");
            }
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                existingRoom.RoomNumber = room.RoomNumber;
                existingRoom.Floor = room.Floor;
                existingRoom.Area = (int)room.Area;
                existingRoom.Price = (int)room.Price;
                existingRoom.Description = room.Description;
                existingRoom.Status = room.Status;
                existingRoom.Amenities = room.Amenities != null ? string.Join(",", room.Amenities) : string.Empty;
                await _roomRepository.UpdateAsync(existingRoom);
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating room");
                transaction.Rollback();
                throw new Exception("Error updating room", ex);
            }
        }
    }
}
