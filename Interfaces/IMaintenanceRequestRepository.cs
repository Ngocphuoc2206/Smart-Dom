using Smart_Dom.DTOs.MaintenanceRequest;
using Smart_Dom.Models;

namespace Smart_Dom.Interfaces
{
    public interface IMaintenanceRequestRepository
    {
        Task<IEnumerable<MaintenanceRequestModel>> GetAllRequestsAsync();
        Task<IEnumerable<MaintenanceRequestViewModel>> GetAllInfoRequestsAsync();
        Task<MaintenanceRequestModel> GetRequestByIdAsync(int id);
        Task<IEnumerable<MaintenanceRequestViewModel>> GetRequestsByUserIdAsync(int userId);
        Task<IEnumerable<MaintenanceRequestModel>> GetRequestsByRoomIdAsync(int roomId);
        Task<IEnumerable<MaintenanceRequestModel>> GetRequestsByPriorityLevelAsync(string priorityLevel);
        Task<IEnumerable<MaintenanceRequestModel>> GetRequestsByStatusAsync(string status);
        Task CreateRequestAsync(MaintenanceRequestModel request);
        Task UpdateRequestAsync(MaintenanceRequestModel request);
        Task DeleteRequestAsync(int id);
        Task<bool> SaveChangesAsync();
    }
}
