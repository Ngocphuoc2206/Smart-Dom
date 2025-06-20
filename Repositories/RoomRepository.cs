using Microsoft.EntityFrameworkCore;
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
                await _context.SaveChangesAsync();
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
