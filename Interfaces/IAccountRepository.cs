using Microsoft.AspNetCore.Mvc;
using Smart_Dom.DTOs;
using Smart_Dom.Models;

namespace Smart_Dom.Interfaces
{
    public interface IAccountRepository
    {
        Task<IEnumerable<AccountModel>> GetAllAsync();
        Task<IEnumerable<AccountRegisterDTO>> GetAllUsersInFormationAsync();
        Task<AccountModel?> GetByIdAsync(int id);
        Task<AccountModel?> GetByUsernameAsync(string username);
        Task CreateAsync(AccountModel account);
        Task DeleteAsync(int id);
        Task<bool> SaveChangesAsync();
    }
}
