using Lagersystem.Blazor.Models.Dtos;
using Lagersystem.Blazor.Models.Requests;
using Lagersystem.Blazor.Services.Abstractions;

namespace Lagersystem.Blazor.Services.Api;

public class OrderApiService : IOrderService
{
    private readonly HttpClient _httpClient;

    public OrderApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<IReadOnlyList<OrderDto>> GetOrdersAsync()
    {
        throw new NotImplementedException();
    }
    public Task<OrderDetailsDto?> GetOrderDetailsByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }
    public Task<OrderDto?> GetOrderByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task CreateOrderAsync(CreateOrderRequest request)
    {
        throw new NotImplementedException();
    }

    public Task UpdateOrderAsync(Guid id, UpdateOrderRequest request)
    {
        throw new NotImplementedException();
    }

    public Task DeleteOrderAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}