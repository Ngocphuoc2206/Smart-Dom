using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Smart_Dom.DTOs.Invoice;
using Smart_Dom.Hubs;
using Smart_Dom.Interfaces;
using Smart_Dom.Models;
using System.Threading.Tasks;

namespace Smart_Dom.Areas.Invoice.Controllers
{
    [Area("Invoice")]
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : Controller
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IContractService _contractService;
        private readonly ILogger<InvoiceController> _logger;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IRoomService _roomService;
        private readonly AppDBContext _context;
        public InvoiceController(IInvoiceService invoiceService, ILogger<InvoiceController> logger, IHubContext<NotificationHub> hubContext, IRoomService roomService, IContractService contractService, AppDBContext context)
        {
            _invoiceService = invoiceService;
            _logger = logger;
            _hubContext = hubContext;
            _roomService = roomService;
            _contractService = contractService;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("Fetching all invoices...");
            var invoices = await _invoiceService.GetInvoicesAsync();
            return Ok(invoices);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTenantInvoice(int id)
        {
            _logger.LogInformation("Fetching all invoices tenant...");
            var invoices = await _invoiceService.GetAllInvoiceByUserID(id);
            return Ok(invoices);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateInvoice invoice)
        {
            _logger.LogInformation("Creating a new invoice");
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for invoice creation");
                return BadRequest(ModelState);
            }
            await _invoiceService.CreateInvoiceAsync(invoice);
            return Ok();
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateBill([FromBody]EditInvoice invoice, int id)
        {
            _logger.LogInformation($"Update Invoice with ID: {id}");
            if (!ModelState.IsValid) {
                _logger.LogWarning("Invalid model state for invoice creation");
                return BadRequest(ModelState);
            }
            //Check existing invoice
            var existingInvoice = await _invoiceService.GetInvoiceByIdAsync(id);
            if (existingInvoice == null) 
            {
                _logger.LogInformation($"Invoice with id {id} doesn't exist");
                return BadRequest(ModelState);
            }
            existingInvoice.Id = id;
            await _invoiceService.UpdateInvoiceAsync(invoice, id);
            return Ok();
        }

        [HttpPost("reminder/{id}")]
        public async Task<IActionResult> SendReminder(int id)
        {
            _logger.LogInformation($"Sending notification to tenant with invoiceID: {id}");
            var invoice = await _invoiceService.GetInvoiceByIdAsync(id);

            if (invoice == null)
                return NotFound("Invoice not found");

            var contract = await _contractService.GetContractByIdAsync(invoice.ContractID);
            var userId = contract.IDUser.ToString();
            var room = await _roomService.GetRoomByIdAsync(contract.RoomId);
            if (room == null)
            {
                return NotFound("Room not found");
            }
            var message = $"Bạn có hóa đơn cần thanh toán cho phòng {room.RoomNumber}.";
            _logger.LogInformation($"ReceiveNotification { message}, UserID: {userId} ");
            await _hubContext.Clients.Group(userId).SendAsync("ReceiveNotification", message);

            return Ok(new { message = "Đã gửi nhắc nhở thành công" });
        }

        [HttpPost("payment/internal")]
        public async Task<IActionResult> SimulatePayment([FromBody] InternalPaymentRequest req)
        {
            // 1. Kiểm tra invoice
            var invoice = await _invoiceService.GetInvoiceByIdAsync(req.InvoiceId);
            if (invoice == null || invoice.Status == "paid")
                return BadRequest("Hóa đơn không hợp lệ");

            // 2. Tạo transaction giả lập
            var txn = new TransactionModel
            {
                InvoiceId = invoice.Id,
                Method = req.Method,
                Status = "success",
                PaidAt = DateTime.Now,
                TransactionCode = $"TXN{DateTimeOffset.Now.ToUnixTimeMilliseconds()}"
            };
            _context.Transactions.Add(txn);

            // 3. Cập nhật invoice
            invoice.Status = "paid";
            await _invoiceService.UpdateAsync(invoice);

            return Ok(new
            {
                Message = "Thanh toán thành công",
                TransactionCode = txn.TransactionCode
            });
        }


    }
}
