using Smart_Dom.DTOs.User;
using Smart_Dom.Models;

namespace Smart_Dom.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserModel>> GetAllUsersAsync();
        Task<UserModel?> GetUserByIdAsync(int id);
        Task<UserModel> RegisterUserAsync(AccountRegisterUserDTO user);
        Task CreateTenantAsync(EditUserDTO user);
        Task DeleteUserAsync(int id);
        Task UpdateUserInFoAsync(int id, EditUserDTO account);
        Task<UserModel?> AuthenticateUserAsync(string username, string password);

    }
}
