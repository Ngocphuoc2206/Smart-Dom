using Microsoft.AspNetCore.Mvc;
using Smart_Dom.DTOs.Invoice;
using Smart_Dom.Services;
using System.Threading.Tasks;

namespace Smart_Dom.Areas.Invoice.Controllers
{
    [Area("Invoice")]
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : Controller
    {
        private readonly IInvoiceService _invoiceService;
        private readonly ILogger<InvoiceController> _logger;
        public InvoiceController(IInvoiceService invoiceService, ILogger<InvoiceController> logger)
        {
            _invoiceService = invoiceService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("Fetching all invoices...");
            var invoices = await _invoiceService.GetInvoicesAsync();
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
    }
}
