using Smart_Dom.DTOs.Invoice;
using Smart_Dom.Models;

namespace Smart_Dom.Services
{
    public interface IInvoiceService
    {
        Task<IEnumerable<InvoiceViewModel>> GetInvoicesAsync();
        Task<InvoiceModel> GetInvoiceByIdAsync(int id);
        Task<IEnumerable<InvoiceModel>> GetInvoicesByContractIdAsync(int contractId);
        Task<IEnumerable<InvoiceModel>> GetInvoicesByStatusAsync(string status);
        Task<IEnumerable<InvoiceModel>> GetInvoicesByInvoiceTypeAsync(string invoiceType);
        Task CreateInvoiceAsync(CreateInvoice invoice);
        Task UpdateInvoiceAsync(EditInvoice invoice, int id);
        Task DeleteInvoiceAsync(int id);
    }
}
