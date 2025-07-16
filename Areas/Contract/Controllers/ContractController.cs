using Microsoft.AspNetCore.Mvc;
using Smart_Dom.DTOs.Contract;
using Smart_Dom.DTOs.Invoice;
using Smart_Dom.Interfaces;
using Smart_Dom.Services;

namespace Smart_Dom.Areas.Contract.Controllers
{
    [Area("Contract")]
    [Route("api/[controller]")]
    [ApiController]
    public class ContractController: Controller
    {
        private readonly IContractService _contractService;
        private readonly ILogger<ContractController> _logger;
        private readonly IRoomBookingService _roomBookingService;
        private readonly IRoomService _roomService;
        public ContractController(IContractService contractService, ILogger<ContractController> logger, IRoomBookingService roomBookingService, IRoomService roomService)
        {
            _contractService = contractService;
            _logger = logger;
            _roomBookingService = roomBookingService;
            _roomService = roomService;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("Retrieving all accounts");
            var contracts = await _contractService.GetAllContractsAsync();
            return Ok(contracts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("Retrieving contract with ID: {Id}", id);
            var contract = await _contractService.GetContractByIdAsync(id);
            if (contract == null)
            {
                _logger.LogWarning("Contract with ID: {Id} not found", id);
                return NotFound();
            }
            return Ok(contract);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateContractDTO contractDto)
        {
            _logger.LogInformation("Creating a new contract" + contractDto.DurationContractID);
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for contract creation");
                return BadRequest(ModelState);
            }
            try
            {
                await _contractService.CreateContractAsync(contractDto);
                _logger.LogInformation("Contract created successfully");
                return Ok("Contract created successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating contract");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateContract([FromBody] UpdateContractDueDate contractDate, int id)
        {
            _logger.LogInformation($"Update Invoice with ID user: {id} Date: {contractDate.ExpiredDate}");
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for invoice creation");
                return BadRequest(ModelState);
            }
            // Check existing invoice
            var existingContract = await _contractService.GetContractByUserIdAsync(id);
            var existingRoomBookings = await _roomBookingService.GetBookingsByUserIdAsync(id); 
            var existingRooms = await _roomService.GetRoomByIdAsync(existingRoomBookings.RoomId);
            if (existingContract == null)
            {
                _logger.LogInformation($"Contract with id {id} doesn't exist");
                return BadRequest(ModelState);
            }

            if (existingRooms == null)
            {
                _logger.LogInformation($"Contract with id {id} doesn't exist");
                return BadRequest(ModelState);
            }

            if (existingRoomBookings == null)
            {
                _logger.LogInformation($"Contract with id {id} doesn't exist");
                return BadRequest(ModelState);
            }

            // Fix for CS0266 and CS8629
            if (contractDate.ExpiredDate.HasValue)
            {
                existingContract.EndDate = contractDate.ExpiredDate.Value;
                existingRoomBookings.DesiredEnd = contractDate.ExpiredDate.Value;
                existingRoomBookings.Status = "occupied";
                existingRooms.Status = "occupied";
                existingContract.Status = "paid";
            }
            else
            {
                _logger.LogWarning("ExpiredDate is null");
                return BadRequest("ExpiredDate cannot be null.");
            }

            await _contractService.UpdateContractByUserIDAsync(id, existingContract);
            return Ok();
        }
    }
}
