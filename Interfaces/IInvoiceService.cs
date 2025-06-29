using Smart_Dom.DTOs.Invoice;
using Smart_Dom.Models;

namespace Smart_Dom.Interfaces
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
        Task UpdateAsync(InvoiceModel invoice);
        Task DeleteInvoiceAsync(int id);
    }
}
