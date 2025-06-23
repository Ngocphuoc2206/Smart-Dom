using Microsoft.EntityFrameworkCore;
using Smart_Dom.DTOs;
using Smart_Dom.Interfaces;
using Smart_Dom.Models;

namespace Smart_Dom.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly AppDBContext _context;

        // Constructor to inject the AppDBContext
        public RoomRepository(AppDBContext context)
        {
            _context = context;
        }
        public async Task CreateAsync(RoomModel room)
        {
            await _context.Rooms.AddAsync(room);
            SaveChangesAsync().Wait();
        }

        public async Task DeleteAsync(int id)
        {
            var existingRoom = await _context.Rooms.FindAsync(id);
            if (existingRoom != null)
            {
                _context.Rooms.Remove(existingRoom);
                SaveChangesAsync().Wait();
            }
            else
            {
                throw new KeyNotFoundException($"Room with ID {id} not found.");
            }
        }

        public async Task<IEnumerable<RoomModel>> GetAllAsync()
        {
            return await _context.Rooms.ToListAsync();
        }

        public Task<IEnumerable<RoomDTO>> GetAllWithHistoryAsync()
        {
            var rooms = from r in _context.Rooms
                        join h in _context.RoomHistories on r.ID equals h.RoomId into roomHistories
                        from h in roomHistories.DefaultIfEmpty()
                        join u in _context.Users on h.UserId equals u.ID into users
                        from u in users.DefaultIfEmpty()
                        select new RoomDTO
                        {
                            ID = r.ID,
                            RoomNumber = r.RoomNumber,
                            Floor = r.Floor,
                            Area = r.Area,
                            Price = r.Price,
                            FullName = u != null ? u.FullName : null,
                            Description = r.Description,
                            Status = r.Status,
                            Amenities = r.Amenities != null ? r.Amenities.Split(new[] { ',' }, StringSplitOptions.None).ToList() : new List<string>() // Fix for CS0854
                        };

            return Task.FromResult(rooms.AsEnumerable());
        }

        public async Task<RoomModel?> GetByIdAsync(int id)
        {
            return await _context.Rooms.FindAsync(id);
        }


        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task UpdateAsync(RoomModel room)
        {
            _context.Rooms.Update(room);
            SaveChangesAsync().Wait();
        }
    }
}
