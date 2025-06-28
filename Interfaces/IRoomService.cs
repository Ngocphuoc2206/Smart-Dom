using Smart_Dom.DTOs.Room;
using Smart_Dom.Models;

namespace Smart_Dom.Interfaces
{
    public interface IRoomService
    {
        Task<IEnumerable<RoomModel>> GetAllRoomAsync();
        Task<IEnumerable<RoomDTO>> GetAllRoomsWithHistoryAsync();
        Task<RoomModel?> GetRoomByIdAsync(int id);
        Task<RoomModel?> GetRoomByRoomNumberAsync(int roomNumber);
        Task CreateRoomAsync(RoomDTO room);
        Task UpdateRoomAsync(RoomDTO room);
        Task UpdateRoom(RoomModel room);
        Task DeleteRoomAsync(int id);

    }
}
