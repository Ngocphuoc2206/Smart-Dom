using Smart_Dom.DTOs.Contract;
using Smart_Dom.Models;

namespace Smart_Dom.Interfaces
{
    public interface IContractService
    {
        Task<IEnumerable<ContractModel>> GetAllContractsAsync();
        Task<ContractModel> GetContractByIdAsync(int id);
        Task<ContractModel?> GetContractByUserIdAsync(int userId);
        Task<ContractModel?> GetContractByRoomIdAsync(int roomId);
        Task CreateContractAsync(CreateContractDTO contract);
        Task UpdateContractAsync(CreateContractDTO contract);
        Task UpdateContractByUserIDAsync(int userId, ContractModel contract);
        Task DeleteContractAsync(int id);
        Task<bool> SaveChangesAsync();
    }
}
