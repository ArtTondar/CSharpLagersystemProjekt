using API.Models;
using API.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _repo;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IOrderRepository repo, ILogger<OrderController> logger)
        {
            _repo = repo;
            _logger = logger;
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving order with id {OrderId}", id);
                return StatusCode(500, $"An error occured while retrieving order: {ex.Message}");
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving orders");
                return StatusCode(500, $"An error occurred while retrieving orders: {ex.Message}");
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving orders for customer {CustomerId}", customerId);
                return StatusCode(500, $"An error occurred while retrieving orders: {ex.Message}");
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving orders by date range");
                return StatusCode(500, $"An error occurred while retrieving orders: {ex.Message}");
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving orders by total price {TotalPrice}", totalPrice);
                return StatusCode(500, $"An error occurred while retrieving orders: {ex.Message}");
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
                return CreatedAtAction(nameof(GetOrderById), new { id = createdOrder.Id }, createdOrder);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating order");
                return StatusCode(500, $"An error occurred while creating order: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(Guid id, [FromBody] Order order)
        {
            // Ændret:
            // Logger er tilføjet og exception returneres med message i development-style.
            //
            // Hvorfor:
            // Den tidligere catch skjulte den rigtige fejl og gjorde det svært at finde årsagen til 500.
            // Nu kan server-log og klientbesked vise hvad der faktisk går galt.

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != order.Id)
            {
                return BadRequest();
            }

            Order? existingOrder = await _repo.GetById(id);

            if (existingOrder == null)
            {
                return NotFound();
            }

            order.OrderDetails ??= new List<OrderDetail>();

            try
            {
                await _repo.Update(order);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating order with id {OrderId}", id);
                return StatusCode(500, $"An error occurred while updating order: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting order with id {OrderId}", id);
                return StatusCode(500, $"An error occurred while deleting order: {ex.Message}");
            }
        }
    }
}