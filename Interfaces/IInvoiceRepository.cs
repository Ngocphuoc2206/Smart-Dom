using Smart_Dom.DTOs.Invoice;
using Smart_Dom.Models;

namespace Smart_Dom.Interfaces
{
    public interface IInvoiceRepository
    {
        Task<IEnumerable<InvoiceViewModel>> GetAllInvoicesAsync();
        Task<InvoiceModel?> GetInvoiceByIdAsync(int id);
        Task<IEnumerable<InvoiceViewModel>> GetAllianceByUserIdAsync(int id);
        Task<IEnumerable<InvoiceModel>> GetInvoicesByContractIdAsync(int contractId);
        Task<IEnumerable<InvoiceModel>> GetInvoicesByStatusAsync(string status);
        Task<IEnumerable<InvoiceModel>> GetInvoicesByInvoiceTypeAsync(string invoiceType);
        Task CreateInvoiceAsync(InvoiceModel invoice);
        Task UpdateInvoiceAsync(InvoiceModel invoice);
        Task DeleteInvoiceAsync(int id);
        Task<bool> SaveChangesAsync();
    }
}
