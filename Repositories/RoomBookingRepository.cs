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
            SaveChangesAsync().Wait();
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
                              select new RoomBookingViewModel
                              {
                                  Id = rb.Id,
                                  FullName = u.FullName,
                                  Phone = u.Phone,
                                  RoomNumber = r.RoomNumber,
                                  DesiredStart = rb.DesiredStart, // Keep as DateTime
                                  DesiredEnd = rb.DesiredEnd, // Keep as DateTime
                                  Status = r.Status
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
