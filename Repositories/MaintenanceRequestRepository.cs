using Microsoft.EntityFrameworkCore;
using Smart_Dom.DTOs.MaintenanceRequest;
using Smart_Dom.Interfaces;
using Smart_Dom.Models;

namespace Smart_Dom.Repositories
{
    public class MaintenanceRequestRepository : IMaintenanceRequestRepository
    {
        private readonly AppDBContext _context;
        public MaintenanceRequestRepository(AppDBContext context)
        {
            _context = context;
        }
        public async Task CreateRequestAsync(MaintenanceRequestModel request)
        {
            await _context.MaintenanceRequests.AddAsync(request);
            await SaveChangesAsync();
        }

        public async Task DeleteRequestAsync(int id)
        {
            var existMaintenanceRequest = await _context.MaintenanceRequests.FirstOrDefaultAsync(x => x.Id == id);
            if (existMaintenanceRequest != null)
            {
                _context.MaintenanceRequests.Remove(existMaintenanceRequest);
                await SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException("Error Delete Maintenance Request");
            }
        }

        public async Task<IEnumerable<MaintenanceRequestViewModel>> GetAllInfoRequestsAsync()
        {
            var requests = from rq in _context.MaintenanceRequests
                           join r in _context.Rooms on rq.RoomId equals r.ID
                           join u in _context.Users on rq.UserId equals u.ID
                           select new MaintenanceRequestViewModel
                           {
                               ID = rq.Id,
                               CreateAt = rq.RequestDate,
                               RoomNumber = r.RoomNumber,
                               Tenant = u.FullName,
                               IncidentType = rq.IncidentType,
                               PriorityLevel = rq.PriorityLevel,
                               Description = rq.Description,
                               Status = rq.Status
                           };
            return await requests.ToListAsync();
        }

        public async Task<IEnumerable<MaintenanceRequestModel>> GetAllRequestsAsync()
        {
            return await _context.MaintenanceRequests.ToListAsync();
        }

        public Task<MaintenanceRequestModel> GetRequestByIdAsync(int id)
        {
            return _context.MaintenanceRequests
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<MaintenanceRequestModel>> GetRequestsByPriorityLevelAsync(string priorityLevel)
        {
            return await _context.MaintenanceRequests.Where(i => i.PriorityLevel == priorityLevel).ToListAsync();
        }

        public async Task<IEnumerable<MaintenanceRequestModel>> GetRequestsByRoomIdAsync(int roomId)
        {
            return await _context.MaintenanceRequests.Where(i => i.RoomId == roomId).ToListAsync();
        }

        public async Task<IEnumerable<MaintenanceRequestModel>> GetRequestsByStatusAsync(string status)
        {
            return await _context.MaintenanceRequests.Where(i => i.Status == status).ToListAsync();
        }

        public async Task<IEnumerable<MaintenanceRequestViewModel>> GetRequestsByUserIdAsync(int userId)
        {
            var requests = from rq in _context.MaintenanceRequests
                           join r in _context.Rooms on rq.RoomId equals r.ID
                           join u in _context.Users on rq.UserId equals u.ID
                           where rq.UserId == userId
                           select new MaintenanceRequestViewModel
                           {
                               ID = rq.Id,
                               CreateAt = rq.RequestDate,
                               RoomNumber = r.RoomNumber,
                               Tenant = u.FullName,
                               IncidentType = rq.IncidentType,
                               PriorityLevel = rq.PriorityLevel,
                               Description = rq.Description,
                               Status = rq.Status
                           };
            return await requests.ToListAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task UpdateRequestAsync(MaintenanceRequestModel request)
        {
            _context.MaintenanceRequests.Update(request);
            await _context.SaveChangesAsync();
        }
    }
}
