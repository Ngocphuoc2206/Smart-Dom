using Smart_Dom.DTOs.Contract;
using Smart_Dom.Models;

namespace Smart_Dom.Services
{
    public interface IContractService
    {
        Task<IEnumerable<ContractModel>> GetAllContractsAsync();
        Task<ContractModel> GetContractByIdAsync(int id);
        Task CreateContractAsync(CreateContractDTO contract);
        Task UpdateContractAsync(CreateContractDTO contract);
        Task DeleteContractAsync(int id);
        Task<bool> SaveChangesAsync();
    }
}
