using API.Models;
using API.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomerController : Controller
    {
        private readonly ICustomerRepository _repo;
        public CustomerController(ICustomerRepository repo)
        {
            _repo = repo;
        }
        private IActionResult OkOrNotFound<T>(List<T> list)
        {
            return (list == null || !list.Any()) ? NotFound() : Ok(list);
        }

        [HttpGet("get-customer-by-id/{id}")]
        public async Task<IActionResult> GetCustomerById(Guid id)
        {
            try
            {
                Customer? customer = await _repo.GetById(id);
                if (customer == null)
                {
                    return NotFound();
                }
                return Ok(customer);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occured while retrieving customer data.");
            }
        }


        [HttpGet("get-customer-by-email/{email}")]
        public async Task<IActionResult> GetCustomerByEmail(string email)
        {
            try
            {
                Customer? customer = await _repo.GetByEmail(email);
                if (customer == null)
                {
                    return NotFound();
                }
                return Ok(customer);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occured while retrieving customer data.");
            }
        }

        [HttpGet("get-customer-by-phone/{phone}")]
        public async Task<IActionResult> GetCustomerByPhone(string phone)
        {
            try
            {
                Customer? customer = await _repo.GetByPhone(phone);
                if (customer == null)
                {
                    return NotFound();
                }
                return Ok(customer);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occured while retrieving customer data.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomers()
        {
            try
            {
                List<Customer> customers = await _repo.GetAll();

                return OkOrNotFound(customers);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving customers.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                Customer createdCustomer = await _repo.Create(customer);
                return CreatedAtAction(nameof(GetCustomerById), new { id = createdCustomer.Id }, createdCustomer);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while creating customer.");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(Guid id, [FromBody] Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != customer.Id)
            {
                return BadRequest();
            }

            var existingCustomer = await _repo.GetById(id);
            if (existingCustomer == null)
            {
                return NotFound();
            }

            try
            {
                await _repo.Update(customer);
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while updating customer.");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            Customer? existingCustomer = await _repo.GetById(id);
            if (existingCustomer == null)
            {
                return NotFound();
            }

            try
            {
                await _repo.Delete(existingCustomer);
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while deleting customer.");
            }
        }
    }
}
