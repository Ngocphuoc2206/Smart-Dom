using Microsoft.EntityFrameworkCore;
using Smart_Dom.DTOs.RoomBooking;
using Smart_Dom.Interfaces;
using Smart_Dom.Models;

namespace Smart_Dom.Repositories
{
    public class RoomBookingRepository : IRoomBookingRepository
    {
        private readonly AppDBContext _context;
        public RoomBookingRepository(AppDBContext context)
        {
            _context = context;
        }
        public async Task AddBookingAsync(RoomBookingModel booking)
        {
            await _context.RoomBookings.AddAsync(booking);
            await SaveChangesAsync();
        }

        public async Task DeleteBookingAsync(int id)
        {
            var existingBooking = await _context.RoomBookings.FindAsync(id);
            if (existingBooking == null)
            {
                throw new KeyNotFoundException($"Booking with ID {id} not found.");
            }
            _context.RoomBookings.Remove(existingBooking);
            await SaveChangesAsync();
        }

        public async Task<IEnumerable<RoomBookingModel>> GetAllBookingsAsync()
        {
            return await _context.RoomBookings.ToListAsync();
        }

        public async Task<RoomBookingModel?> GetBookingByIdAsync(int id)
        {
            return await _context.RoomBookings.FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<RoomBookingModel>> GetBookingsByRoomIdAsync(int roomId)
        {
            return await _context.RoomBookings
                .Where(b => b.RoomId == roomId)
                .ToListAsync();
        }

        public async Task<IEnumerable<RoomBookingModel>> GetBookingsByUserIdAsync(int userId)
        {
            return await _context.RoomBookings
                .Where(b => b.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<RoomBookingViewModel>> GetRoomAllInformation()
        {
            var bookingRoom = from rb in _context.RoomBookings
                              join u in _context.Users on rb.UserId equals u.ID
                              join r in _context.Rooms on rb.RoomId equals r.ID
                              join c in _context.Contracts on new { RoomId = rb.RoomId, UserId = u.ID }
                              equals new { RoomId = c.RoomId, UserId = c.IDUser } into contractGroup
                              from c in contractGroup.DefaultIfEmpty() // Ensure 'c' is in scope here
                              join dc in _context.DurationContracts on c.DurationContractID equals dc.ID into durationGroup
                              from dc in durationGroup.DefaultIfEmpty() // Ensure 'dc' is in scope here
                              select new RoomBookingViewModel
                              {
                                  Id = rb.Id,
                                  FullName = u.FullName,
                                  Phone = u.Phone,
                                  RoomNumber = r.RoomNumber,
                                  Email = u.Email,
                                  price = r.Price,
                                  NumberID = u.IDCard,
                                  UserId = u.ID,
                                  DesiredStart = rb.DesiredStart,
                                  DepositAmount = c.DepositAmount,
                                  DesiredEnd = rb.DesiredEnd,
                                  Status = r.Status,
                                  EmergencyContact = u.EmergencyContact,
                                  EmergencyPhone = u.EmergencyPhone,
                                  DurationContract = dc.Duration
                              };
            return await bookingRoom.ToListAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task UpdateBooking(RoomBookingModel booking)
        {
            _context.RoomBookings.Update(booking);
            await SaveChangesAsync();
        }

    }
}
