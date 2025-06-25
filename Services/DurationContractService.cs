using Smart_Dom.Interfaces;
using Smart_Dom.Models;

namespace Smart_Dom.Services
{
    public class DurationContractService : IDurationContracService
    {
        private readonly IDurationContractRepository _durationContractRepository;
        private readonly AppDBContext _context;
        private readonly ILogger<DurationContractService> _logger;
        public DurationContractService(IDurationContractRepository durationContractRepository, AppDBContext context, ILogger<DurationContractService> logger)
        {
            _durationContractRepository = durationContractRepository;
            _context = context;
            _logger = logger;
        }
        public async Task CreateAsync(DurationContractModel durationContract)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                _logger.LogInformation($"Creating new duration contract with ID {durationContract.ID}");
                await _durationContractRepository.CreateAsync(durationContract);
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating duration contract");
                await transaction.RollbackAsync();
                throw new Exception("Error creating duration contract", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                _logger.LogInformation($"Deleting duration contract with ID {id}");
                await _durationContractRepository.DeleteAsync(id);
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting duration contract");
                await transaction.RollbackAsync();
                throw new Exception("Error deleting duration contract", ex);
            }
        }

        public async Task<IEnumerable<DurationContractModel>> GetAllDurationContractAsync()
        {
            return await _durationContractRepository.GetAllAsync();
        }

        public async Task<DurationContractModel?> GetByIdAsync(int id)
        {
            return await _durationContractRepository.GetByIdAsync(id);
        }

        public Task UpdateAsync(DurationContractModel durationContract)
        {
            throw new NotImplementedException();
        }
    }
}
