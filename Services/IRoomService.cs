using Smart_Dom.DTOs;
using Smart_Dom.Models;

namespace Smart_Dom.Services
{
    public interface IRoomService
    {
        Task<IEnumerable<RoomModel>> GetAllRoomAsync();
        Task<IEnumerable<RoomDTO>> GetAllRoomsWithHistoryAsync();
        Task<RoomModel?> GetRoomByIdAsync(int id);
        Task CreateRoomAsync(RoomDTO room);
        Task UpdateRoomAsync(RoomDTO room, int id);
        Task DeleteRoomAsync(int id);

    }
}
