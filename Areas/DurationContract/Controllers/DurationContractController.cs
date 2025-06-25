using Microsoft.AspNetCore.Mvc;
using Smart_Dom.Services;
using System.Threading.Tasks;

namespace Smart_Dom.Areas.DurationContract.Controllers
{
    [Area("DurationContract")]
    [Route("api/[controller]")]
    [ApiController]
    public class DurationContractController: Controller
    {
        private readonly ILogger<DurationContractController> _logger;
        private readonly IDurationContracService _durationContracService;
        public DurationContractController(ILogger<DurationContractController> logger, IDurationContracService durationContracService)
        {
            _logger = logger;
            _durationContracService = durationContracService;
        }

        [HttpGet]
        public async Task<IActionResult> Get() {
            var result = await _durationContracService.GetAllDurationContractAsync();
            return Ok(result);
        }
    }
}
