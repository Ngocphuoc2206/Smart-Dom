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
        private readonly ICheckInHistoryRepository _checkInHistoryRepository;
        private readonly IDurationContractRepository _durationContractRepository;
        private readonly ILogger<UserService> _logger;
        private readonly AppDBContext _context;
        public UserService(IUserRepository userRepository, ILogger<UserService> logger, AppDBContext context, 
            IRoomRepository roomRepository, IContractRepository contractRepository, 
            IDurationContractRepository durationContractRepository, IRoomBookingRepository roomBookingRepository, 
            ICheckInHistoryRepository checkInHistoryRepository)
        {
            _userRepository = userRepository;
            _logger = logger;
            _context = context;
            _roomRepository = roomRepository;
            _contractRepository = contractRepository;
            _durationContractRepository = durationContractRepository;
            _roomBookingRepository = roomBookingRepository;
            _checkInHistoryRepository = checkInHistoryRepository;
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

                var newCheckInHistory = new CheckInHistoryModel
                {
                    UserId = newUser.ID,
                    RoomId = room.ID,
                    CheckInTime = user.DesiredStart,
                    CheckOutTime = user.DesiredStart.AddMonths(user.DurationContract),
                    Status = "checked-in",
                };
                await _checkInHistoryRepository.CreateCheckInHistoryAsync(newCheckInHistory);
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
            _logger.LogInformation("Deleting user with ID: {Id}", id);
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                //update the checkin history status to cancelled
                var checkInHistory = await _checkInHistoryRepository.GetCheckInTimesByUserIdAsync(id);
                if (checkInHistory == null)
                {
                    _logger.LogWarning("No check-in history found for user with ID {Id}.", id);
                    throw new KeyNotFoundException($"No check-in history found for user with ID {id}.");
                }
                checkInHistory.Status = "cancelled";
                await _checkInHistoryRepository.UpdateCheckInHistoryAsync(checkInHistory);

                //Update the room status
                var roomBooking = await _roomBookingRepository.GetBookingsByUserIdAsync(id);
                if (roomBooking == null)
                {
                    _logger.LogWarning("No room booking found for user with ID {Id}.", id);
                    throw new KeyNotFoundException($"No room booking found for user with ID {id}.");
                }
                roomBooking.Status = "cancelled";
                await _roomBookingRepository.UpdateBooking(roomBooking);
                //update the room status to available
                var room = await _roomRepository.GetByIdAsync(roomBooking.RoomId);
                if (room == null)
                {
                    _logger.LogWarning("No room found with ID {RoomId}.", roomBooking.RoomId);
                    throw new KeyNotFoundException($"No room found with ID {roomBooking.RoomId}.");
                }
                room.Status = "available";
                await _roomRepository.UpdateAsync(room);

                //Update the contrract status
                var contract = await _contractRepository.GetByUserIdAsync(id);
                if (contract == null)
                {
                    _logger.LogWarning("No contract found for user with ID {Id}.", id);
                    throw new KeyNotFoundException($"No contract found for user with ID {id}.");
                }
                contract.Status = "cancelled";
                await _contractRepository.UpdateAsync(contract);
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting user with ID {Id}.", id);
                await transaction.RollbackAsync();
                throw;
            }

            //await _userRepository.DeleteAsync(id);
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
