using Smart_Dom.Models;

namespace Smart_Dom.Interfaces
{
    public interface IMaintenanceRequestRepository
    {
        Task<IEnumerable<MaintenanceRequest>> GetAllRequestsAsync();
        Task<MaintenanceRequest> GetRequestByIdAsync(int id);
        Task<IEnumerable<MaintenanceRequest>> GetRequestsByUserIdAsync(int userId);
        Task<IEnumerable<MaintenanceRequest>> GetRequestsByPropertyIdAsync(int propertyId);
        Task<IEnumerable<MaintenanceRequest>> GetRequestsByStatusAsync(string status);
        Task CreateRequestAsync(MaintenanceRequest request);
        Task UpdateRequestAsync(MaintenanceRequest request);
        Task DeleteRequestAsync(int id);
        Task<bool> SaveChangesAsync();
    }
}
