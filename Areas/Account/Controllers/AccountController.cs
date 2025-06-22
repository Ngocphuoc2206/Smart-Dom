using BCrypt.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Smart_Dom.DTOs;
using Smart_Dom.Interfaces;
using Smart_Dom.Models;
using Smart_Dom.Services;
using System.Threading.Tasks;
namespace Smart_Dom.Areas.AccountControllers.Controllers
{
    [Area("AccountControllers")]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly AppDBContext _context;
        private readonly IAccountService _accountService;
        private readonly IUserService _userService;
        public AccountController(ILogger<AccountController> logger, AppDBContext context, IAccountService accountService, IUserService userService)
        {
            _logger = logger;
            _context = context;
            _accountService = accountService;
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogInformation("Retrieving all accounts");
            var accounts = _accountService.GetAllUsersInFormationAsync();
            return Ok(accounts);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AccountRegisterDTO account)
        {
            _logger.LogInformation("Creating a new account");
            // Validate the model state
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for account creation");
                return BadRequest(ModelState);
            }
            try
            {
                await _accountService.RegisterAccountAsync(account);
                return Ok("Account registered successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while registering account");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModelDTO model)
        {
            _logger.LogInformation("User login attempt for email: {Email}", model.Email);
            var account = await _accountService.GetAccountByUsernameAsync(model.Email);
            //Decode the password and verify it against the stored hash
            if (account == null || !BCrypt.Net.BCrypt.Verify(model.Password, account.PasswordHash))
            {
                return Unauthorized("Invalid email or password.");
            }

            var user = await _userService.GetUserByIdAsync(account.UserId);
            return Ok(new
            {
                username = account.UserName,
                role = account.Role,
                fullName = user?.FullName,
                email = user?.Email,
                idUser = account?.UserId,
                phone = user?.Phone,
                dob = user?.DOB.ToString("yyyy-MM-dd"), // Format date to string
                address = user?.Address,
                gender = user?.Gender,
                idNumber = user?.IDCard,
            });
        }
    }
}
