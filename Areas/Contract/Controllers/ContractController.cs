using Microsoft.AspNetCore.Mvc;
using Smart_Dom.DTOs.Contract;
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
        public ContractController(IContractService contractService, ILogger<ContractController> logger)
        {
            _contractService = contractService;
            _logger = logger;
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
    }
}
