using Smart_Dom.Models;

namespace Smart_Dom.Interfaces
{
    public interface IRoomRepository
    {
        Task CreateAsync(RoomModel room);
        Task DeleteAsync(int id);
        Task<IEnumerable<RoomModel>> GetAllAsync();
        Task<RoomModel?> GetByIdAsync(int id);
        Task UpdateAsync(RoomModel room);
        Task<bool> SaveChangesAsync();
    }
}
