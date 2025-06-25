using Smart_Dom.Models;

namespace Smart_Dom.Interfaces
{
    public interface IDurationContractRepository
    {
        Task<IEnumerable<DurationContractModel>> GetAllAsync();
        Task<DurationContractModel?> GetByIdAsync(int id);
        Task<DurationContractModel?> GetByDurationAsync(int duration);
        Task CreateAsync(DurationContractModel durationContract);
        Task UpdateAsync(DurationContractModel durationContract);
        Task DeleteAsync(int id);
        Task<bool> SaveChangesAsync();
    }
}
