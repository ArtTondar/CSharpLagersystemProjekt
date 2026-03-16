using Lagersystem.Blazor.API.Clients;
using Lagersystem.Blazor.Models.Dtos;
using Lagersystem.Blazor.Services.Abstractions;

namespace Lagersystem.Blazor.Services.Api;

public class CustomerApiService : ICustomerService
{
    private readonly ApiClient _apiClient;

    public CustomerApiService(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<IReadOnlyList<CustomerDto>> GetCustomersAsync()
    {
        // Placeholder til at hente alle kunder.
        // Aktivér denne når Customer endpointet findes i API'et.
        // Hvis endpoint-navnet bliver anderledes, skal URL'en rettes her.

        return await _apiClient.GetAsync<List<CustomerDto>>("api/Customer")
               ?? new List<CustomerDto>();
    }

    public async Task<CustomerDto?> GetCustomerByIdAsync(Guid id)
    {
        // Placeholder til at hente én kunde ud fra id.
        // Denne metode er nyttig, hvis man senere vil have en kundedetaljeside.

        return await _apiClient.GetAsync<CustomerDto>($"api/Customer/{id}");
    }
}