using Smart_Dom.DTOs;
using Smart_Dom.DTOs.User;
using Smart_Dom.Interfaces;
using Smart_Dom.Models;
using Smart_Dom.Repositories;

namespace Smart_Dom.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IRoomBookingRepository _roomBookingRepository;
        private readonly IContractRepository _contractRepository;
        private readonly IDurationContractRepository _durationContractRepository;
        private readonly ILogger<UserService> _logger;
        private readonly AppDBContext _context;
        public UserService(IUserRepository userRepository, ILogger<UserService> logger, AppDBContext context, IRoomRepository roomRepository, IContractRepository contractRepository, IDurationContractRepository durationContractRepository, IRoomBookingRepository roomBookingRepository)
        {
            _userRepository = userRepository;
            _logger = logger;
            _context = context;
            _roomRepository = roomRepository;
            _contractRepository = contractRepository;
            _durationContractRepository = durationContractRepository;
            _roomBookingRepository = roomBookingRepository;
        }
        public Task<UserModel?> AuthenticateUserAsync(string username, string password)
        {
            throw new NotImplementedException();
        }

        public async Task CreateTenantAsync(EditUserDTO user)
        {
            _logger.LogInformation("Creating a new tenant with email: {Email}", user.Email);
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var newUser = new UserModel()
                {
                    FullName = user.FullName,
                    Email = user.Email,
                    Phone = user.Phone,
                    IDCard = user.NumberID,
                    EmergencyPhone = user.EmergencyPhone,
                    EmergencyContact = user.EmergencyContact,
                };
                await _userRepository.CreateAsync(newUser);
                var room = await _roomRepository.GetByRoomNumberAsync(user.RoomNumber);
                if (room == null)
                {
                    _logger.LogWarning("Room with number {RoomNumber} does not exist.", user.RoomNumber);
                    throw new KeyNotFoundException($"Room with number {user.RoomNumber} does not exist.");
                }
                room.Status = "occupied";
                await _roomRepository.UpdateAsync(room);
                _logger.LogInformation("Room with number {RoomNumber} found.", user.RoomNumber, "ID: ", room.ID);
                
                var newRoomBooking = new RoomBookingModel
                {
                    RoomId = room.ID,
                    UserId = newUser.ID,
                    DesiredStart = user.DesiredStart,
                    DesiredEnd = DateTime.Now.AddMonths(user.DurationContract),
                    Status = "occupied"
                };
                await _roomBookingRepository.AddBookingAsync(newRoomBooking);
                _logger.LogInformation("Room booking created for user {UserId} in room {RoomId}.", newUser.ID, room.ID);
                //Find the duration contract by ID
                var durationContract = await _durationContractRepository.GetByDurationAsync(user.DurationContract);
                if (durationContract == null)
                {
                    _logger.LogWarning("Duration contract with ID {DurationContractId} does not exist.", user.DurationContract);
                    throw new KeyNotFoundException($"Duration contract with ID {user.DurationContract} does not exist.");
                }
                var newContract = new ContractModel
                {
                    StartDate = user.DesiredStart,
                    EndDate = DateTime.Now.AddMonths(user.DurationContract),
                    IDUser = newUser.ID,
                    RoomId = room.ID,
                    Status = "pending",
                    DepositAmount = user.DepositAmount ?? 0,
                    DurationContractID = durationContract.ID
                };
                await _contractRepository.CreateAsync(newContract);
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                _logger.LogError("Error creating tenant and booking.");
                throw;
            }
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


        public async Task<UserModel> RegisterUserAsync(AccountRegisterUserDTO accountRegister)
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

        public async Task UpdateUserInFoAsync(int id, EditUserDTO account)
        {
            _logger.LogInformation("Updating user with ID: {Id}", id);
            var existingUser = await _userRepository.GetByIdAsync(id);
            if (existingUser == null)
            {
                _logger.LogWarning("User with ID {Id} not found for update.", id);
                throw new KeyNotFoundException($"User with ID {id} not found.");
            }
            existingUser.FullName = account.FullName;
            existingUser.Email = account.Email;
            existingUser.Phone = account.Phone;
            existingUser.IDCard = account.NumberID;
            existingUser.EmergencyPhone = account.EmergencyPhone;
            existingUser.EmergencyContact = account.EmergencyContact;
            await _userRepository.UpdateAsync(existingUser);
        }
    }
}
