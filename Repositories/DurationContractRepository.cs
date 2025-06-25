using Microsoft.EntityFrameworkCore;
using Smart_Dom.Interfaces;
using Smart_Dom.Models;

namespace Smart_Dom.Repositories
{
    public class DurationContractRepository : IDurationContractRepository
    {
        private readonly AppDBContext _context;
        public DurationContractRepository(AppDBContext context)
        {
            _context = context;
        }
        public async Task CreateAsync(DurationContractModel durationContract)
        {
            await _context.DurationContracts.AddAsync(durationContract);
            await SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existingDurationContract = await _context.DurationContracts.FindAsync(id);
            if (existingDurationContract != null)
            {
                _context.DurationContracts.Remove(existingDurationContract);
                await SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException($"Duration contract with ID {id} not found.");
            }
        }

        public async Task<IEnumerable<DurationContractModel>> GetAllAsync()
        {
            return await _context.DurationContracts.ToListAsync();
        }

        public async Task<DurationContractModel?> GetByDurationAsync(int duration)
        {
            return await _context.DurationContracts
                .FirstOrDefaultAsync(dc => dc.Duration == duration);
        }

        public async Task<DurationContractModel?> GetByIdAsync(int id)
        {
            return await _context.DurationContracts.FirstOrDefaultAsync(dc => dc.ID == id);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public Task UpdateAsync(DurationContractModel durationContract)
        {
            throw new NotImplementedException();
        }
    }
}
