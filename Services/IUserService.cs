using Smart_Dom.DTOs;
using Smart_Dom.Models;

namespace Smart_Dom.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserModel>> GetAllUsersAsync();
        Task<UserModel?> GetUserByIdAsync(int id);
        Task<UserModel> RegisterUserAsync(AccountRegisterDTO user);
        Task DeleteUserAsync(int id);
        Task UpdateUserAsync(int id, UserModel account);
        Task<UserModel?> AuthenticateUserAsync(string username, string password);

    }
}
