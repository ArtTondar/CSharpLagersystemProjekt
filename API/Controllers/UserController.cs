using API.Models;
using API.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
