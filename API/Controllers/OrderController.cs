using API.Models;
using API.Repositories.DbAccess;
using API.Repositories.DbAccess.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly IOrderAccess _orderAccess;
        public OrderController(OrderAccess orderAccess)
        {
            _orderAccess = orderAccess;
        }

        [HttpGet("get-order-by-id/{id}")]
        public async Task<IActionResult> GetOrderById(Guid id)
        {
            Order order = await _orderAccess.GetOrderById(id);

            if (order != null)
            {
                return Ok(order);
            }

            return StatusCode(500, "An error occurred while retrieving order.");
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            try
            {
                var orders = await _orderAccess.GetOrders();
                if (orders == null || !orders.Any())
                {
                    return NotFound();
                }
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving orders.");
            }
        }

        [HttpGet("get-orders-by-customer-id/{customerId}")]
        public async Task<IActionResult> GetOrdersByCustomerId(Guid customerId)
        {
            try
            {
                var orders = await _orderAccess.GetOrdersByCustomerId(customerId);
                if (orders == null || !orders.Any())
                {
                    return NotFound();
                }
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving orders.");
            }
        }

        [HttpGet("get-orders-by-date-range")]
        public async Task<IActionResult> GetOrdersByDateRange([FromQuery] DateTime startDate, DateTime endDate)
        {
            try
            {
                var orders = await _orderAccess.GetOrdersByDateRange(startDate, endDate);
                if (orders == null || !orders.Any())
                {
                    return NotFound();
                }
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving orders.");
            }
        }




        }
}
