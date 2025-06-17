using Microsoft.AspNetCore.Mvc;
using Smart_Dom.DTO;
using Smart_Dom.Models;
using System.Threading.Tasks;

namespace Smart_Dom.Areas.AccountControllers.Controllers
{
    [Area("AccountControllers")]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly AppDBContext _context;
        public AccountController(ILogger<AccountController> logger, AppDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                _logger.LogInformation("Retrieving all accounts");
                var accounts = _context.Accounts.ToList();
                return Ok(accounts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving accounts");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AccountDto account)
        {
            _logger.LogInformation("Creating a new account");
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var newAccount = new Account()
                {
                    UserName = account.Email,
                    Password = account.Password,
                    Role = 1, // Assuming 1 is for Tenant
                };
                var newUser = new UserModel()
                {
                    FullName = account.FullName,
                    Email = account.Email,
                    Phone = account.Phone,
                    Gender = account.Gender,
                    IDCard = account.IDCard,
                    Address = account.Address,
                };
                await _context.Users.AddAsync(newUser);
                await _context.Accounts.AddAsync(newAccount);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating account");
                return StatusCode(500, "Internal server error");
            }
            finally
            {
                transaction.Commit();
            }
            return Ok();
        }
    }
}
