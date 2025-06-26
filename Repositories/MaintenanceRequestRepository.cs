using Microsoft.EntityFrameworkCore;
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
        public Task CreateRequestAsync(MaintenanceRequest request)
        {
            throw new NotImplementedException();
        }

        public Task DeleteRequestAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<MaintenanceRequest>> GetAllRequestsAsync()
        {
            return await _context.MaintenanceRequests.ToListAsync();
        }

        public Task<MaintenanceRequest> GetRequestByIdAsync(int id)
        {
            return _context.MaintenanceRequests
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public Task<IEnumerable<MaintenanceRequest>> GetRequestsByPropertyIdAsync(int propertyId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MaintenanceRequest>> GetRequestsByStatusAsync(string status)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MaintenanceRequest>> GetRequestsByUserIdAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateRequestAsync(MaintenanceRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
