using Microsoft.EntityFrameworkCore;
using Smart_Dom.DTOs;
using Smart_Dom.Interfaces;
using Smart_Dom.Models;

namespace Smart_Dom.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AppDBContext _context;

        public AccountRepository(AppDBContext context)
        {
            _context = context;
        }
        public async Task CreateAsync(AccountModel account)
        {
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existingAccount = await _context.Accounts.FindAsync(id);
            if (existingAccount != null)
            {
                _context.Accounts.Remove(existingAccount);
                SaveChangesAsync().Wait();
            }
            else
            {
                throw new KeyNotFoundException($"Account with ID {id} not found.");
            }
        }

        public async Task<IEnumerable<AccountModel>> GetAllAsync()
        {
            return await _context.Accounts.ToListAsync();
        }

        public async Task<IEnumerable<AccountRegisterDTO>> GetAllUsersInFormationAsync()
        {
            var accounts = await (from a in _context.Accounts
                           join u in _context.Users on a.UserId equals u.ID
                           select new
                           {
                               username = a.UserName,
                               password = a.PasswordHash,
                               role = a.Role,
                               fullName = u.FullName,
                               email = u.Email,
                               phone = u.Phone,
                               dob = u.DOB.ToString("yyyy-MM-dd"), // Format date to string
                           }).ToListAsync();
            return (IEnumerable<AccountRegisterDTO>)accounts;
        }

        public async Task<AccountModel?> GetByIdAsync(int id)
        {
            return await _context.Accounts.FindAsync(id);
        }

        public async Task<AccountModel?> GetByUsernameAsync(string username)
        {
            return await _context.Accounts.FirstOrDefaultAsync(u => u.UserName == username);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
