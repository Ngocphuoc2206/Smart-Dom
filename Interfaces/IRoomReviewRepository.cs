using Smart_Dom.DTOs.RoomRevew;
using Smart_Dom.Models;

namespace Smart_Dom.Interfaces
{
    public interface IRoomReviewRepository
    {
        Task<IEnumerable<RoomReviewModel>> GetAllAsync();
        Task<IEnumerable<RoomReviewViewModel>>GetAllInfoAsync();
        Task<RoomReviewModel?> GetByIdAsync(int id);
        Task AddAsync(RoomReviewModel review);
        Task DeleteAsync(int id);
        Task UpdateAsync(RoomReviewModel review);
        Task SaveChangesAsync();
    }

}
