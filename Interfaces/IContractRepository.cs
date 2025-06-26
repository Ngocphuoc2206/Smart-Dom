using Smart_Dom.Models;

namespace Smart_Dom.Interfaces
{
    public interface IContractRepository
    {
        Task<IEnumerable<ContractModel>> GetAllAsync();
        Task<ContractModel> GetByIdAsync(int? id);
        Task<ContractModel> GetByUserIdAsync(int? userId);
        Task<ContractModel> GetByRoomIdAsync(int? roomId);
        Task CreateAsync(ContractModel contract);
        Task UpdateAsync(ContractModel contract);
        Task DeleteAsync(int id);
        Task<bool> SaveChangesAsync();
    }
}
