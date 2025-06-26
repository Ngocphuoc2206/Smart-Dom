using Smart_Dom.Models;

namespace Smart_Dom.Services
{
    public interface ICheckInHistoryService
    {
        Task<IEnumerable<CheckInHistoryModel>> GetAllCheckInTimesAsync();
        Task<CheckInHistoryModel?> GetCheckInTimeByIdAsync(int id);
        Task<IEnumerable<CheckInHistoryModel>> GetCheckInTimesByRoomIdAsync(int roomId);
        Task<CheckInHistoryModel> GetCheckInTimesByUserIdAsync(int userId);
        Task CreateCheckInHistoryAsync(CheckInHistoryModel checkInHistory);
        Task UpdateCheckInHistoryAsync(CheckInHistoryModel checkInHistory);
        Task DeleteCheckInHistoryAsync(int id);
        Task<bool> SaveChangesAsync();
    }
}
