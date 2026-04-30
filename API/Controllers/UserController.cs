using API.Models;
using API.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserRepository _repo;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserRepository repo, ILogger<UserController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        private IActionResult OkOrNotFound(User? user)
        {
            return user == null ? NotFound() : Ok(user);
        }

        [HttpGet("get-user-by-id/{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            try
            {
                User? user = await _repo.GetById(id);
                return OkOrNotFound(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving user with id {UserId}", id);
                return StatusCode(500, "An error occurred while retrieving user.");
            }
        }

        [HttpGet("get-user-by-email/{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            try
            {
                User? user = await _repo.GetByEmail(email);
                return OkOrNotFound(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving user with email {Email}", email);
                return StatusCode(500, "An error occurred while retrieving user.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                List<User> users = await _repo.GetAll();

                if (users == null || !users.Any())
                {
                    return NotFound();
                }

                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving users");
                return StatusCode(500, "An error occurred while retrieving users.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                User createdUser = await _repo.Create(user);
                return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating user");
                return StatusCode(500, "An error occurred while creating user.");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Login data is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
            {
                return BadRequest("Email and password are required.");
            }

            try
            {
                User? user = await _repo.GetByEmail(dto.Email);

                if (user == null || user.Password != dto.Password)
                {
                    return Unauthorized("Invalid email or password.");
                }

                string role = user.IsAdmin ? "Admin" : "User";

                List<Claim> claims = new()
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, role),
                    new Claim("IsAdmin", user.IsAdmin.ToString())
                };

                ClaimsIdentity identity = new(claims, "Cookies");
                ClaimsPrincipal principal = new(identity);

                await HttpContext.SignInAsync("Cookies", principal);

                CurrentUserDto result = new()
                {
                    Name = user.Name,
                    Email = user.Email,
                    Role = role,
                    IsAuthenticated = true,
                    IsAdmin = user.IsAdmin
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for email {Email}", dto.Email);
                return StatusCode(500, "An error occurred during login.");
            }
        }

        [HttpGet("me")]
        public IActionResult GetCurrentUser()
        {
            try
            {
                if (User?.Identity?.IsAuthenticated != true)
                {
                    return Unauthorized();
                }

                CurrentUserDto result = new()
                {
                    Name = User.FindFirstValue(ClaimTypes.Name) ?? string.Empty,
                    Email = User.FindFirstValue(ClaimTypes.Email) ?? string.Empty,
                    Role = User.FindFirstValue(ClaimTypes.Role) ?? "User",
                    IsAuthenticated = true,
                    IsAdmin = bool.TryParse(User.FindFirst("IsAdmin")?.Value, out bool isAdmin) && isAdmin
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving current user");
                return StatusCode(500, "An error occurred while retrieving current user.");
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync("Cookies");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during logout");
                return StatusCode(500, "An error occurred during logout.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.Id)
            {
                return BadRequest();
            }

            User? existingUser = await _repo.GetById(id);

            if (existingUser == null)
            {
                return NotFound();
            }

            try
            {
                await _repo.Update(user);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating user with id {UserId}", id);
                return StatusCode(500, "An error occurred while updating user.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            User? existingUser = await _repo.GetById(id);

            if (existingUser == null)
            {
                return NotFound();
            }

            try
            {
                await _repo.Delete(existingUser);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting user with id {UserId}", id);
                return StatusCode(500, "An error occurred while deleting user.");
            }
        }
    }
}