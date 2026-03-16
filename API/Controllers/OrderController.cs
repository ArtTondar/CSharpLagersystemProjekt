using API.Models;
using API.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _repo;
        public OrderController(IOrderRepository repo)
        {
            _repo = repo;
        }
        private IActionResult OkOrNotFound<T>(List<T> list)
        {
            return (list == null || !list.Any()) ? NotFound() : Ok(list);
        }

        [HttpGet("get-order-by-id/{id}")]
        public async Task<IActionResult> GetOrderById(Guid id)
        {
            try
            {
                Order? order = await _repo.GetById(id);
                if (order == null)
                {
                    return NotFound();
                }
                return Ok(order);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occured while retrieving order.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            try
            {
                List<Order> orders = await _repo.GetAll();

                return OkOrNotFound(orders);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving orders.");
            }
        }

        [HttpGet("get-orders-by-customer-id/{customerId}")]
        public async Task<IActionResult> GetOrdersByCustomerId(Guid customerId)
        {
            try
            {
                List<Order> orders = await _repo.GetByCustomerId(customerId);
                return OkOrNotFound(orders);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving orders.");
            }
        }

        [HttpGet("get-orders-by-date")]
        public async Task<IActionResult> GetOrdersByDateRange([FromQuery] DateTime startDate, DateTime endDate)
        {
            try
            {
                List<Order> orders = await _repo.GetByDate(startDate, endDate);
                return OkOrNotFound(orders);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving orders.");
            }
        }

        [HttpGet("get-orders-by-total-price/{totalPrice}")]
        public async Task<IActionResult> GetOrdersByTotalPrice(decimal totalPrice)
        {
            try
            {
                List<Order> orders = await _repo.GetByTotalPrice(totalPrice);
                return OkOrNotFound(orders);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving orders.");
            }

        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                Order createdOrder = await _repo.Create(order);
                return CreatedAtAction(nameof(GetOrderById), new { id = createdOrder.Id}, createdOrder);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while creating order.");
            }
        }

        [HttpPut("/{id}")]
        public async Task<IActionResult> UpdateOrder(Guid id, Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != order.Id)
            {
                return BadRequest();
            }

            var existingOrder = await _repo.GetById(id);
            if (existingOrder == null)
            {
                return NotFound();
            } 

            try
            {
                await _repo.Update(order);
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while updating order.");
            }
        }

        [HttpDelete("/{id}")]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            Order? existingOrder = await _repo.GetById(id);
            if (existingOrder == null)
            {
                return NotFound();
            }

            try
            {
                await _repo.Delete(existingOrder);
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while deleting order.");
            }
        }
    }
}
