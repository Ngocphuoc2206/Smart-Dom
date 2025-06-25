using Smart_Dom.DTOs.Contract;
using Smart_Dom.Interfaces;
using Smart_Dom.Models;

namespace Smart_Dom.Services
{
    public class ContractService : IContractService
    {
        private readonly IContractRepository _contractRepository;
        private readonly ILogger<ContractService> _logger;
        private readonly AppDBContext _context;
        public ContractService(IContractRepository contractRepository, ILogger<ContractService> logger, AppDBContext context)
        {
            _contractRepository = contractRepository;
            _logger = logger;
            _context = context;
        }
        public async Task CreateContractAsync(CreateContractDTO contract)
        {
            _logger.LogInformation("Creating a new contract: " + contract.DurationContractID);
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var newContract = new ContractModel
                {
                    StartDate = contract.StartDate,
                    EndDate = contract.EndDate,
                    IDUser = contract.UserId,
                    RoomId = contract.RoomId,
                    Status = contract.Status,
                    DepositAmount = contract.DepositAmount,
                    DurationContractID = contract.DurationContractID

                };
                await _contractRepository.CreateAsync(newContract);
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error creating contract");
                throw;
            }
        }

        public Task DeleteContractAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ContractModel>> GetAllContractsAsync()
        {
            return await _contractRepository.GetAllAsync();
        }

        public async Task<ContractModel> GetContractByIdAsync(int id)
        {
            return await _contractRepository.GetByIdAsync(id);
        }

        public async Task<ContractModel?> GetContractByRoomIdAsync(int roomId)
        {
            return await _contractRepository.GetByRoomIdAsync(roomId);
        }

        public async Task<ContractModel?> GetContractByUserIdAsync(int userId)
        {
            return await _contractRepository.GetByUserIdAsync(userId);
        }

        public Task<bool> SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateContractAsync(CreateContractDTO contract)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateContractByUserIDAsync(int userId, ContractModel contract)
        {
            var existingContract = await _contractRepository.GetByUserIdAsync(userId);
            if (existingContract == null)
            {
                throw new Exception($"No contract found for user ID {userId}");
            }
            existingContract.StartDate = contract.StartDate;
            existingContract.DepositAmount = contract.DepositAmount;
            existingContract.EndDate = contract.EndDate;
            existingContract.Status = contract.Status;
            await _contractRepository.UpdateAsync(existingContract);
        }
    }
}
