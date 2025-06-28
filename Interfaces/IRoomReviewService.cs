using Smart_Dom.DTOs.RoomRevew;
using Smart_Dom.Models;

namespace Smart_Dom.Interfaces
{
    public interface IRoomReviewService
    {
        Task<IEnumerable<RoomReviewModel>> GetAllReviewsAsync();
        Task<IEnumerable<RoomReviewViewModel>> GetAllReviewsViewModelAsync();
        Task<RoomReviewModel?> GetReviewByIdAsync(int id);
        Task AddReviewAsync(RoomReviewModel review);
        Task DeleteReviewAsync(int id);
        Task UpdateReviewAsynx(RoomReviewModel review);
    }

}
