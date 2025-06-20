using Microsoft.EntityFrameworkCore;
using Smart_Dom.Interfaces;
using Smart_Dom.Models;
using System.Net.WebSockets;

namespace Smart_Dom.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDBContext _context;
        // Constructor to inject the AppDBContext
        public UserRepository(AppDBContext context)
        {
            _context = context;
        }
        public async Task CreateAsync(UserModel user)
        {
            await _context.Users.AddAsync(user);
            await SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                SaveChangesAsync().Wait();
            }
            else
            {
                throw new KeyNotFoundException($"User with ID {id} not found.");
            }

        }

        public async Task<IEnumerable<UserModel>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<UserModel?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task UpdateAsync(UserModel user)
        {
            _context.Users.Update(user);
            await SaveChangesAsync();
        }
    }
}
