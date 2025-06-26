using Smart_Dom.DTOs.Room;
using Smart_Dom.Models;

namespace Smart_Dom.Interfaces
{
    public interface IRoomRepository
    {
        Task CreateAsync(RoomModel room);
        Task DeleteAsync(int id);
        Task<IEnumerable<RoomModel>> GetAllAsync();
        Task<IEnumerable<RoomDTO>> GetAllWithHistoryAsync();
        Task<RoomModel?> GetByIdAsync(int id);
        Task<RoomModel?> GetByRoomNumberAsync(int? roomNumber);
        Task UpdateAsync(RoomModel room);
        Task<bool> SaveChangesAsync();
    }
}
