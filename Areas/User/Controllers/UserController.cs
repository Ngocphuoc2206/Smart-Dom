using Microsoft.AspNetCore.Mvc;
using Smart_Dom.DTOs.User;
using Smart_Dom.Models;
using Smart_Dom.Services;

namespace Smart_Dom.Areas.User.Controllers
{
    [Area("User")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController: Controller
    {
        private readonly IUserService _userService;
        private readonly IContractService _contractService;
        private readonly ILogger<UserController> _logger;
        public UserController(IUserService userService, ILogger<UserController> logger, IContractService contractService)
        {
            _userService = userService;
            _logger = logger;
            _contractService = contractService;
        }
        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving users");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GetUserById/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound($"User with ID {id} not found.");
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user by ID");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateUser([FromBody] EditUserDTO user)
        {
            _logger.LogInformation("Creating a new user");
            if (user == null)
            {
                return BadRequest("User data is null.");
            }
            try
            {
                await _userService.CreateTenantAsync(user);
                _logger.LogInformation("User created successfully");
                return Ok("User created successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating user");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] EditUserDTO user)
        {
            _logger.LogInformation("Updating user with ID: {Id}", id);
            if (user == null || user.Id != id)
            {
                return BadRequest("User data is invalid or ID mismatch.");
            }
            try
            {
                await _userService.UpdateUserInFoAsync(id, user);

                var contract = new ContractModel()
                {
                    DepositAmount = user.DepositAmount ?? 0, // Explicitly handle nullable value
                };

                await _contractService.UpdateContractByUserIDAsync(id, contract);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, "User not found for update");
                return NotFound($"User with ID {id} not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
