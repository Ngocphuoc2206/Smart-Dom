using Smart_Dom.DTOs;
using Smart_Dom.Interfaces;
using Smart_Dom.Models;
using Smart_Dom.Repositories;

namespace Smart_Dom.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;
        public UserService(IUserRepository userRepository, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }
        public Task<UserModel?> AuthenticateUserAsync(string username, string password)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteUserAsync(int id)
        {
            await _userRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<UserModel>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<UserModel?> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }


        public async Task<UserModel> RegisterUserAsync(AccountRegisterDTO accountRegister)
        {
            _logger.LogInformation("Registering new user with email: {Email}", accountRegister.Email);
            var user = new UserModel()
            {
                FullName = accountRegister.FullName,
                Email = accountRegister.Email,
                Phone = accountRegister.Phone,
                DOB = accountRegister.DOB,
                Address = accountRegister.Address,
                Gender = accountRegister.Gender,
                IDCard = accountRegister.IDCard
            };
            await _userRepository.CreateAsync(user);
            return user;
        }

        public Task UpdateUserAsync(int id, UserModel user)
        {
            throw new NotImplementedException();
        }
    }
}
