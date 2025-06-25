using Microsoft.CodeAnalysis.CSharp.Syntax;
using Smart_Dom.DTOs.RoomBooking;
using Smart_Dom.Interfaces;
using Smart_Dom.Models;

namespace Smart_Dom.Services
{
    public class RoomBookingService : IRoomBookingService
    {
        private readonly IRoomBookingRepository _bookingRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IUserRepository _userRepository;
        private readonly AppDBContext _context;
        private readonly ILogger<RoomBookingService> _logger;
        public RoomBookingService(IRoomBookingRepository bookingRepository, ILogger<RoomBookingService> logger, AppDBContext context, IRoomRepository roomRepository, IUserRepository userRepository)
        {
            _bookingRepository = bookingRepository;
            _context = context;
            _logger = logger;
            _roomRepository = roomRepository;
            _userRepository = userRepository;
        }
        public async Task AddBookingAsync(CreateRoomBookingDTO booking)
        {
            var existingBooking = await _bookingRepository.GetBookingByIdAsync(booking.RoomId);
            var existingRoom = await _roomRepository.GetByIdAsync(booking.RoomId);
            if (existingRoom == null)
            {
                _logger.LogWarning($"Room with ID {booking.RoomId} does not exist.");
                throw new KeyNotFoundException($"Room with ID {booking.RoomId} does not exist.");
            }
            if (existingBooking != null)
            {
                _logger.LogWarning($"Booking for room {booking.RoomId} already exists.");
                throw new InvalidOperationException($"Booking for room {booking.RoomId} already exists.");
            }else if (booking.DesiredStart < DateTime.Now)
            {
                _logger.LogWarning("Desired start date cannot be in the past.");
                throw new ArgumentException("Desired start date cannot be in the past.");
            }
            else if (booking.ContractDuration <= 0)
            {
                _logger.LogWarning("Contract duration must be greater than zero.");
                throw new ArgumentException("Contract duration must be greater than zero.");
            }
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                    var newBooking = new RoomBookingModel
                    {
                        RoomId = booking.RoomId,
                        UserId = booking.UserId,
                        DesiredStart = booking.DesiredStart,
                        DesiredEnd = booking.DesiredStart.AddDays(booking.ContractDuration),
                        Status = "pending" // Default status, can be changed later
                    };
                _logger.LogInformation($"Adding new booking for room {booking.RoomId} by user {booking.UserId}");
                //Update Room Status
                existingRoom.Status = "pending";
                await _bookingRepository.AddBookingAsync(newBooking);
                await _context.SaveChangesAsync();
                await _roomRepository.UpdateAsync(existingRoom);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error adding booking");
                throw new Exception("Error adding booking", ex);
            }
        }

        public async Task AddBookingTenantAsync(RoomBookingViewModel booking)
        {
            var newUser = new UserModel
            {
                FullName = booking.FullName,
                Email = booking.Email,
                Phone = booking.Phone,
                IDCard = booking.NumberID,
                EmergencyContact = booking.EmergencyContact,
                EmergencyPhone = booking.EmergencyPhone
            };

            //Find room by room number
            var room = await _roomRepository.GetByRoomNumberAsync(booking.RoomNumber);
        }

        public Task DeleteBookingAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<RoomBookingModel>> GetAllBookingsAsync()
        {
            return await _bookingRepository.GetAllBookingsAsync();
        }

        public async Task<RoomBookingModel?> GetBookingByIdAsync(int id)
        {
            return await _bookingRepository.GetBookingByIdAsync(id);
        }

        public Task<IEnumerable<RoomBookingModel>> GetBookingsByRoomIdAsync(int roomId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<RoomBookingModel>> GetBookingsByUserIdAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<RoomBookingViewModel>> GetRoomAllInformation()
        {
            return await _bookingRepository.GetRoomAllInformation();
        }

        public async Task UpdateBookingAsync(RoomBookingModel booking)
        {
            await _bookingRepository.UpdateBooking(booking);
        }

    }
}
