using Microsoft.EntityFrameworkCore;
using Smart_Dom.Interfaces;
using Smart_Dom.Models;

namespace Smart_Dom.Repositories
{
    public class CheckInHistoryRepository : ICheckInHistoryRepository
    {
        private readonly AppDBContext _context;
        public CheckInHistoryRepository(AppDBContext context)
        {
            _context = context;
        }
        public async Task CreateCheckInHistoryAsync(CheckInHistoryModel checkInHistory)
        {
            await _context.CheckInHistories.AddAsync(checkInHistory);
            await SaveChangesAsync();
        }

        public async Task DeleteCheckInHistoryAsync(int id)
        {
            var checkInHistory = await _context.CheckInHistories.FindAsync(id);
            if (checkInHistory != null)
            {
                _context.CheckInHistories.Remove(checkInHistory);
                await SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException($"Check-in history with ID {id} not found.");
            }
        }

        public async Task<IEnumerable<CheckInHistoryModel>> GetAllCheckInTimesAsync()
        {
            return await _context.CheckInHistories.ToListAsync();
        }

        public async Task<CheckInHistoryModel?> GetCheckInTimeByIdAsync(int id)
        {
            return await _context.CheckInHistories
                .FirstOrDefaultAsync(c => c.ID == id);
        }

        public async Task<IEnumerable<CheckInHistoryModel>> GetCheckInTimesByRoomIdAsync(int roomId)
        {
            return (IEnumerable<CheckInHistoryModel>)await _context.CheckInHistories.FirstOrDefaultAsync(c => c.RoomId == roomId);
        }

        public async Task<IEnumerable<CheckInHistoryModel>> GetCheckInTimesByUserIdAsync(int userId)
        {
            return (IEnumerable<CheckInHistoryModel>)await _context.CheckInHistories.FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public Task UpdateCheckInHistoryAsync(CheckInHistoryModel checkInHistory)
        {
            throw new NotImplementedException();
        }
    }
}
