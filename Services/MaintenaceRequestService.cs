using Smart_Dom.DTOs.MaintenanceRequest;
using Smart_Dom.Interfaces;
using Smart_Dom.Models;

namespace Smart_Dom.Services
{
    public class MaintenaceRequestService : IMaintenanceRequestService
    {
        private readonly IMaintenanceRequestRepository _maintenanceRequestRepository;
        private readonly ILogger<MaintenaceRequestService> _logger;
        private readonly AppDBContext _context;
        public MaintenaceRequestService(IMaintenanceRequestRepository maintenanceRequestRepository, 
            ILogger<MaintenaceRequestService> logger, AppDBContext context)
        {
            _maintenanceRequestRepository = maintenanceRequestRepository;
            _logger = logger;
            _context = context;
        }

        public async Task CreateRequestAsync(CreateMaintenanceRequestDTO request)
        {
            _logger.LogInformation("Create Maintenance Request");
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var newRequest = new MaintenanceRequestModel()
                {
                    UserId = request.UserId,
                    RoomId = request.RoomId,
                    RequestDate = DateTime.Now,
                    PriorityLevel = request.PriorityLevel,
                    Location = request.Location,
                    IncidentType = request.IncidentType,
                    Description = request.Description,
                    Status = "pending processing"
                };
                await _maintenanceRequestRepository.CreateRequestAsync(newRequest);
                await transaction.CommitAsync();
            }
            catch (Exception ex) 
            {
                await transaction.RollbackAsync();
                _logger.LogError("Error create Maintenance Request");
                throw new Exception("Error create Request");
            }
        }

        public Task DeleteRequestAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<MaintenanceRequestViewModel>> GetAllInfoRequestsAsync()
        {
            return await _maintenanceRequestRepository.GetAllInfoRequestsAsync();
        }

        public async Task<IEnumerable<MaintenanceRequestModel>> GetAllRequestsAsync()
        {
            return await _maintenanceRequestRepository.GetAllRequestsAsync();
        }

        public async Task<MaintenanceRequestModel> GetRequestByIdAsync(int id)
        {
            return await _maintenanceRequestRepository.GetRequestByIdAsync(id);
        }

        public async Task<IEnumerable<MaintenanceRequestModel>> GetRequestsByPriorityLevelAsync(string priorityLevel)
        {
            return await _maintenanceRequestRepository.GetRequestsByPriorityLevelAsync(priorityLevel);
        }

        public async Task<IEnumerable<MaintenanceRequestModel>> GetRequestsByRoomIdAsync(int roomId)
        {
            return await _maintenanceRequestRepository.GetRequestsByRoomIdAsync(roomId);
        }

        public async Task<IEnumerable<MaintenanceRequestModel>> GetRequestsByStatusAsync(string status)
        {
            return await _maintenanceRequestRepository.GetRequestsByStatusAsync(status);
        }

        public async Task<IEnumerable<MaintenanceRequestModel>> GetRequestsByUserIdAsync(int userId)
        {
            return await _maintenanceRequestRepository.GetRequestsByUserIdAsync(userId);
        }

        public async Task HandleRequestAsync(MaintenanceRequestModel request)
        {
            await _maintenanceRequestRepository.UpdateRequestAsync(request);
        }

        public Task UpdateRequestAsync(CreateMaintenanceRequestDTO request)
        {
            throw new NotImplementedException();
        }
    }
}
