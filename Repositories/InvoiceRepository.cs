using Microsoft.EntityFrameworkCore;
using Smart_Dom.DTOs.Invoice;
using Smart_Dom.Interfaces;
using Smart_Dom.Models;
using System.Diagnostics.Contracts;

namespace Smart_Dom.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly AppDBContext _context;

        public InvoiceRepository(AppDBContext context)
        {
            _context = context;

        }
        public async Task CreateInvoiceAsync(InvoiceModel invoice)
        {
            await _context.Invoices.AddAsync(invoice);
            await SaveChangesAsync();
        }

        public async Task DeleteInvoiceAsync(int id)
        {
            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice != null)
            {
                _context.Invoices.Remove(invoice);
                await SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException($"Invoice with ID {id} not found.");
            }
        }

        public async Task<IEnumerable<InvoiceViewModel>> GetAllInvoicesAsync()
        {
            var invoices = from i in _context.Invoices
                           join ct in _context.Contracts on i.ContractID equals ct.ID
                           join r in _context.Rooms on ct.RoomId equals r.ID
                           join u in _context.Users on ct.IDUser equals u.ID
                           join t in _context.Transactions on i.Id equals t.InvoiceId
                           select new InvoiceViewModel()
                           { 
                                Id = i.Id,
                                UserId = ct.IDUser,
                                RoomNumber = r.RoomNumber,
                                Tenant = u.FullName,
                                RentRooms = r.Price,
                                InvoiceAmount = i.TotalAmount,
                                InvoiceType = i.InvoiceType,
                                ElectricUsage = i.ElectricUsage,
                                ServiceFees = i.ServiceFees,
                                Method = t.Method,
                                PaidAt = t.PaidAt,
                                TransactionCode = t.TransactionCode,
                                WaterUsage = i.WaterUsage,
                                Note = i.Note,
                                InvoiceDateLimit = i.InvoiceDateLimit,
                                Status = i.Status,
                           };
            return invoices;
        }

        public async Task<InvoiceModel?> GetInvoiceByIdAsync(int id)
        {
            return await _context.Invoices.FindAsync(id);
        }

        public async Task<IEnumerable<InvoiceModel>> GetInvoicesByContractIdAsync(int contractId)
        {
            var invoices = await _context.Invoices
                .Where(i => i.ContractID == contractId)
                .ToListAsync();

            return invoices ?? Enumerable.Empty<InvoiceModel>();
        }

        public async Task<IEnumerable<InvoiceModel>> GetInvoicesByInvoiceTypeAsync(string invoiceType)
        {
            var invoices = await _context.Invoices
                .Where(i => i.InvoiceType == invoiceType)
                .ToListAsync();

            return invoices ?? Enumerable.Empty<InvoiceModel>();
        }

        public async Task<IEnumerable<InvoiceModel>> GetInvoicesByStatusAsync(string status)
        {
            var invoices = await _context.Invoices
                .Where(i => i.Status == status)
                .ToListAsync();

            return invoices ?? Enumerable.Empty<InvoiceModel>();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task UpdateInvoiceAsync(InvoiceModel invoice)
        {
            _context.Update(invoice);
            await SaveChangesAsync();
        }
    }
}
