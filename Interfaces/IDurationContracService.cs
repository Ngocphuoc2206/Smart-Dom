using Smart_Dom.Models;

namespace Smart_Dom.Interfaces
{
    public interface IDurationContracService
    {
        Task<IEnumerable<DurationContractModel>> GetAllDurationContractAsync();
        Task<DurationContractModel?> GetByIdAsync(int id);
        Task CreateAsync(DurationContractModel durationContract);
        Task UpdateAsync(DurationContractModel durationContract);
        Task DeleteAsync(int id);

    }
}
