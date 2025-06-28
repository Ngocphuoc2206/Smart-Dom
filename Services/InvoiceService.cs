using Smart_Dom.DTOs.Invoice;
using Smart_Dom.Interfaces;
using Smart_Dom.Models;

namespace Smart_Dom.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IContractRepository _contractRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly ILogger<InvoiceService> _logger;
        private readonly AppDBContext _context;
        public InvoiceService(IInvoiceRepository invoiceRepository, ILogger<InvoiceService> logger, AppDBContext context, IContractRepository contractRepository, IRoomRepository roomRepository)
        {
            _invoiceRepository = invoiceRepository;
            _logger = logger;
            _context = context;
            _contractRepository = contractRepository;
            _roomRepository = roomRepository;
        }
        public async Task CreateInvoiceAsync(CreateInvoice invoice)
        {
           using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _logger.LogInformation("Creating a new invoice with details: {@Invoice}", invoice);

                if (invoice == null)
                {
                    _logger.LogError("Invoice data is null.");
                    throw new ArgumentNullException(nameof(invoice), "Invoice data cannot be null.");
                }

                if (string.IsNullOrEmpty(invoice.InvoiceType))
                {
                    _logger.LogError("InvoiceType is null or empty.");
                    throw new ArgumentException("InvoiceType cannot be null or empty.", nameof(invoice.InvoiceType));
                }

                var room = await _roomRepository.GetByRoomNumberAsync(invoice.RoomNumber);

                if (room == null)
                {
                    _logger.LogError($"Room {invoice.RoomNumber}");
                    throw new ArgumentException("RoomNumber cannot be null");
                }

                var contract = await _contractRepository.GetByRoomIdAsync(room.ID);
                if (contract == null)
                {
                    _logger.LogError("Contract not found for RoomID: {RoomID}", room.ID);
                    throw new KeyNotFoundException($"Contract not found for RoomID: {room.ID}");
                }

                var newInvoice = new InvoiceModel
                {
                    ContractID = contract.ID,
                    InvoiceType = invoice.InvoiceType!,
                    InvoiceDateLimit = invoice.InvoiceDateLimit,
                    ElectricUsage = invoice.ElectricUsage,
                    ServiceFees = invoice.ServiceFees,
                    WaterUsage = invoice.WaterUsage,
                    TotalAmount = invoice.TotalAmount,
                    Note = invoice.Note,
                    Status = "pending"
                };

                _logger.LogInformation("Creating invoice: {@NewInvoice}", newInvoice);
                await _invoiceRepository.CreateInvoiceAsync(newInvoice);
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating invoice");
                await transaction.RollbackAsync();
                throw;
            }

        }

        public async Task DeleteInvoiceAsync(int id)
        {
            var invoice = await _invoiceRepository.GetInvoiceByIdAsync(id);
            if (invoice == null)
            {
                _logger.LogError("Invoice with ID {Id} not found.", id);
                throw new KeyNotFoundException($"Invoice with ID {id} not found.");
            }
            _logger.LogInformation("Deleting invoice with ID {Id}", id);
            await _invoiceRepository.DeleteInvoiceAsync(id);
        }

        public async Task<InvoiceModel> GetInvoiceByIdAsync(int id)
        {
            return await _invoiceRepository.GetInvoiceByIdAsync(id);
        }

        public async Task<IEnumerable<InvoiceViewModel>> GetInvoicesAsync()
        {
            return await _invoiceRepository.GetAllInvoicesAsync();
        }

        public async Task<IEnumerable<InvoiceModel>> GetInvoicesByContractIdAsync(int contractId)
        {
            return await _invoiceRepository.GetInvoicesByContractIdAsync(contractId);
        }

        public async Task<IEnumerable<InvoiceModel>> GetInvoicesByInvoiceTypeAsync(string invoiceType)
        {
            return await _invoiceRepository.GetInvoicesByInvoiceTypeAsync(invoiceType);
        }

        public async Task<IEnumerable<InvoiceModel>> GetInvoicesByStatusAsync(string status)
        {
            return await _invoiceRepository.GetInvoicesByStatusAsync(status);
        }


        public async Task UpdateInvoiceAsync(EditInvoice invoice, int id)
        {
            if (invoice == null)
            {
                _logger.LogError("Invalid invoice data provided for update.");
                throw new ArgumentNullException(nameof(invoice), "Invoice data cannot be null and must have a valid ID.");
            }

            var existingInvoice = await _invoiceRepository.GetInvoiceByIdAsync(id);
            if (existingInvoice == null)
            {
                _logger.LogError("Invoice with ID {Id} not found.", id);
                throw new KeyNotFoundException($"Invoice with ID {id} not found.");
            }

            _logger.LogInformation("Updating invoice with ID {Id}", id);

            existingInvoice.InvoiceType = invoice.InvoiceType ?? existingInvoice.InvoiceType;
            existingInvoice.InvoiceDateLimit = invoice.InvoiceDateLimit;
            existingInvoice.ElectricUsage = invoice.ElectricUsage;
            existingInvoice.WaterUsage = invoice.WaterUsage;
            existingInvoice.TotalAmount = invoice.TotalAmount;
            existingInvoice.Note = invoice.Note ?? existingInvoice.Note;

            await _invoiceRepository.UpdateInvoiceAsync(existingInvoice);
        }
    }
}
