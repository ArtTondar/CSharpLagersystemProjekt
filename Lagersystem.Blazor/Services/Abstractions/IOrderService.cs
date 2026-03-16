using Lagersystem.Blazor.Models.Dtos;
using Lagersystem.Blazor.Models.Requests;

namespace Lagersystem.Blazor.Services.Abstractions;

public interface IOrderService
{
    Task<IReadOnlyList<OrderDto>> GetOrdersAsync();

    Task<OrderDto?> GetOrderByIdAsync(Guid id);

    Task<IReadOnlyList<OrderDto>> GetOrdersByCustomerIdAsync(Guid customerId);

    Task<IReadOnlyList<OrderDto>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate);

    Task<IReadOnlyList<OrderDto>> GetOrdersByTotalPriceAsync(decimal totalPrice);

    Task CreateOrderAsync(CreateOrderRequest request);

    Task UpdateOrderAsync(Guid id, UpdateOrderRequest request);

    Task DeleteOrderAsync(Guid id);
}