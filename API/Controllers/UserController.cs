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
        public UserController(IUserRepository repo)
        {
            _repo = repo;
        }
        private IActionResult OkOrNotFound(User? user)
        {
            return (user == null) ? NotFound() : Ok(user);
        }

        [HttpGet("get-user-by-id/{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            try
            {
                User? user = await _repo.GetById(id);
                return OkOrNotFound(user);
            }
            catch (Exception)
            {
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
            catch (Exception)
            {
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
            catch (Exception)
            {
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
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while creating user.");
            }
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            User? user = await _repo.GetByEmail(dto.Email);
            if (user == null || user.Password!= dto.Password)
                return Unauthorized();


            var claims = new List<Claim>
        {
            new Claim("username", user.Name),
            new Claim("Role", user.IsAdmin ? "Admin" : "Normal") //xonvert boolean to string
        };

            var identity = new ClaimsIdentity(claims, "Cookies");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("Cookies", principal);

            return Ok();
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
            catch (Exception)
            {
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
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while deleting user.");
            }
        }
    }
}
