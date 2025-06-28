using Smart_Dom.DTOs.User;
using Smart_Dom.Models;

namespace Smart_Dom.Interfaces
{
    public interface IAccountService
    {
        Task<IEnumerable<AccountModel>> GetAllAccountAsync();
        Task<IEnumerable<AccountRegisterUserDTO>> GetAllUsersInFormationAsync();
        Task<AccountModel?> GetAccountByIdAsync(int id);
        Task<AccountModel?> GetAccountByUsernameAsync(string username);
        Task RegisterAccountAsync(AccountRegisterUserDTO account);
        Task DeleteAccountAsync(int id);
        Task UpdateAccountAsync(int id, AccountRegisterUserDTO account);
    }
}
