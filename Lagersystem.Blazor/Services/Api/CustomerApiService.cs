using Lagersystem.Blazor.Models.Dtos;
using Lagersystem.Blazor.Services.Abstractions;

namespace Lagersystem.Blazor.Services.Api;

public class CustomerApiService : ICustomerService
{
    private readonly HttpClient _httpClient;

    public CustomerApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<IReadOnlyList<CustomerDto>> GetCustomersAsync()
    {
        throw new NotImplementedException();
    }

    public Task<CustomerDto?> GetCustomerByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}