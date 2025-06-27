using Smart_Dom.DTOs.RoomRevew;
using Smart_Dom.Interfaces;
using Smart_Dom.Models;

namespace Smart_Dom.Services
{
    public class RoomReviewService : IRoomReviewService
    {
        private readonly IRoomReviewRepository _repository;

        public RoomReviewService(IRoomReviewRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<RoomReviewModel>> GetAllReviewsAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<RoomReviewModel?> GetReviewByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task AddReviewAsync(RoomReviewModel review)
        {
            await _repository.AddAsync(review);
            await _repository.SaveChangesAsync();
        }

        public async Task DeleteReviewAsync(int id)
        {
            await _repository.DeleteAsync(id);
            await _repository.SaveChangesAsync();
        }

        public async Task<IEnumerable<RoomReviewViewModel>> GetAllReviewsViewModelAsync()
        {
            return await _repository.GetAllInfoAsync();
        }

        public async Task UpdateReviewAsynx(RoomReviewModel review)
        {
            await _repository.UpdateAsync(review);
        }
    }

}
