using Smart_Dom.DTOs.User;
using Smart_Dom.Interfaces;
using Smart_Dom.Models;
using Smart_Dom.Repositories;

namespace Smart_Dom.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IUserService _userService;
        private readonly AppDBContext _context;
        private readonly ILogger<AccountService> _logger;
        public AccountService(IAccountRepository accountRepository, IUserService userService, AppDBContext context, ILogger<AccountService> logger)
        {
            _accountRepository = accountRepository;
            _userService = userService;
            _context = context;
            _logger = logger;
        }
        public async Task RegisterAccountAsync(AccountRegisterUserDTO account)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Check if the account already exists
                var existingAccount = await _accountRepository.GetByUsernameAsync(account.Email);
                if (existingAccount != null)
                {
                    throw new Exception("Account with this email already exists.");
                }
                var newUser = await _userService.RegisterUserAsync(account);
                _logger.LogInformation("New user registered with ID: {UserId}", newUser.ID);
                var newAccount = new AccountModel
                {
                    UserName = account.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(account.Password),
                    Role = 1, // Default role, can be changed later
                    UserId = newUser.ID // Assuming UserId is part of the DTO
                };
                await _accountRepository.CreateAsync(newAccount);
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Error registering account", ex);
            }
        }

        public Task DeleteAccountAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<AccountModel?> GetAccountByIdAsync(int id)
        {
            return await _accountRepository.GetByIdAsync(id);
        }

        public async Task<AccountModel?> GetAccountByUsernameAsync(string username)
        {
            return await _accountRepository.GetByUsernameAsync(username);
        }

        public async Task<IEnumerable<AccountModel>> GetAllAccountAsync()
        {
            return await _accountRepository.GetAllAsync();
        }

        public Task UpdateAccountAsync(int id, AccountRegisterUserDTO account)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AccountRegisterUserDTO>> GetAllUsersInFormationAsync()
        {
            return await _accountRepository.GetAllUsersInFormationAsync();
        }
    }
}
