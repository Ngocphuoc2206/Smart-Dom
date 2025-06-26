using Microsoft.EntityFrameworkCore;
using Smart_Dom.Interfaces;
using Smart_Dom.Models;

namespace Smart_Dom.Repositories
{
    public class ContractRepository : IContractRepository
    {
        private readonly AppDBContext _context;
        public ContractRepository(AppDBContext context)
        {
            _context = context;
        }
        public async Task CreateAsync(ContractModel contract)
        {
            await _context.Contracts.AddAsync(contract);
            await SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existingContract = await _context.Contracts.FindAsync(id);
            if (existingContract != null)
            {
                _context.Contracts.Remove(existingContract);
                await SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException($"Contract with ID {id} not found.");
            }
        }
        public async Task<IEnumerable<ContractModel>> GetAllAsync()
        {
            return await _context.Contracts.ToListAsync();
        }

        public async Task<ContractModel?> GetByIdAsync(int? id)
        {
            return await _context.Contracts.FirstOrDefaultAsync(c => c.ID == id);
        }

        public async Task<ContractModel> GetByRoomIdAsync(int? roomId)
        {
            return await _context.Contracts.FirstOrDefaultAsync(c => c.RoomId == roomId);
        }

        public async Task<ContractModel?> GetByUserIdAsync(int? userId)
        {
            return await _context.Contracts.FirstOrDefaultAsync(c => c.IDUser == userId);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task UpdateAsync(ContractModel contract)
        {
            _context.Contracts.Update(contract);
            await SaveChangesAsync();
        }
    }
}
