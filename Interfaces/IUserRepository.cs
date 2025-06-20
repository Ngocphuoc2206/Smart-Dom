using Smart_Dom.Models;

namespace Smart_Dom.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserModel>> GetAllAsync();
        Task<UserModel?> GetByIdAsync(int id);
        Task CreateAsync(UserModel user);
        Task UpdateAsync(UserModel user);
        Task DeleteAsync(int id);
        Task<bool> SaveChangesAsync();
    }
}
