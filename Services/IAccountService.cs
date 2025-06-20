using Smart_Dom.DTOs;
using Smart_Dom.Models;

namespace Smart_Dom.Services
{
    public interface IAccountService
    {
        Task<IEnumerable<AccountModel>> GetAllAccountAsync();
        Task<IEnumerable<AccountRegisterDTO>> GetAllUsersInFormationAsync();
        Task<AccountModel?> GetAccountByIdAsync(int id);
        Task<AccountModel?> GetAccountByUsernameAsync(string username);
        Task RegisterAccountAsync(AccountRegisterDTO account);
        Task DeleteAccountAsync(int id);
        Task UpdateAccountAsync(int id, AccountRegisterDTO account);
    }
}
