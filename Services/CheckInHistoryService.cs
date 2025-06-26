using Smart_Dom.Interfaces;
using Smart_Dom.Models;

namespace Smart_Dom.Services
{
    public class CheckInHistoryService : ICheckInHistoryService
    {
        private readonly ICheckInHistoryRepository _checkInHistoryRepository;
        private readonly ILogger<CheckInHistoryService> _logger;
        private readonly AppDBContext _context;

        public CheckInHistoryService(ICheckInHistoryRepository checkInHistoryRepository, ILogger<CheckInHistoryService> logger, AppDBContext context)
        {
            _checkInHistoryRepository = checkInHistoryRepository;
            _logger = logger;
            _context = context;
        }
        public async Task CreateCheckInHistoryAsync(CheckInHistoryModel checkInHistory)
        {
            _logger.LogInformation("Creating check-in history for user ID {UserId} in room ID {RoomId}", checkInHistory.UserId, checkInHistory.RoomId);
            if (checkInHistory == null)
            {
                _logger.LogError("Check-in history is null");
                throw new ArgumentNullException(nameof(checkInHistory), "Check-in history cannot be null");
            }
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                await _checkInHistoryRepository.CreateCheckInHistoryAsync(checkInHistory);
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error creating check-in history");
                throw;
            }
        }

        public Task DeleteCheckInHistoryAsync(int id)
        {
            _logger.LogInformation("Deleting check-in history with ID {Id}", id);
            return _checkInHistoryRepository.DeleteCheckInHistoryAsync(id);
        }

        public async Task<IEnumerable<CheckInHistoryModel>> GetAllCheckInTimesAsync()
        {
            return await _checkInHistoryRepository.GetAllCheckInTimesAsync();
        }

        public async Task<CheckInHistoryModel?> GetCheckInTimeByIdAsync(int id)
        {
            return await _checkInHistoryRepository.GetCheckInTimeByIdAsync(id);
        }

        public async Task<IEnumerable<CheckInHistoryModel>> GetCheckInTimesByRoomIdAsync(int roomId)
        {
            return await _checkInHistoryRepository.GetCheckInTimesByRoomIdAsync(roomId);
        }

        public async Task<CheckInHistoryModel> GetCheckInTimesByUserIdAsync(int userId)
        {
            return await _checkInHistoryRepository.GetCheckInTimesByUserIdAsync(userId);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _checkInHistoryRepository.SaveChangesAsync();
        }

        public Task UpdateCheckInHistoryAsync(CheckInHistoryModel checkInHistory)
        {
            throw new NotImplementedException();
        }
    }
}
