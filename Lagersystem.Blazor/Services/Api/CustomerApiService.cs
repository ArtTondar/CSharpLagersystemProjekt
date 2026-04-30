//using Lagersystem.Blazor.API.Clients;
//using Lagersystem.Blazor.Models.Dtos;
//using Lagersystem.Blazor.Services.Abstractions;

//namespace Lagersystem.Blazor.Services.Api;

//public class CustomerApiService : ICustomerService
//{
//    private readonly ApiClient _apiClient;

//    public CustomerApiService(ApiClient apiClient)
//    {
//        _apiClient = apiClient;
//    }

//    public async Task<IReadOnlyList<CustomerDto>> GetCustomersAsync()
//    {
//        // Placeholder til at hente alle kunder.
//        // Aktivér denne når Customer endpointet findes i API'et.
//        // Hvis endpoint-navnet bliver anderledes, skal URL'en rettes her.

//        return await _apiClient.GetAsync<List<CustomerDto>>("api/Customer")
//               ?? new List<CustomerDto>();
//    }

//    public async Task<CustomerDto?> GetCustomerByIdAsync(Guid id)
//    {
//        // Placeholder til at hente én kunde ud fra id.
//        // Denne metode er nyttig, hvis man senere vil have en kundedetaljeside.

//        return await _apiClient.GetAsync<CustomerDto>($"api/Customer/{id}");
//    }
//}

using Lagersystem.Blazor.API.Helpers;
using Lagersystem.Blazor.Models.Dtos;
using Lagersystem.Blazor.Services.Abstractions;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using System.Net.Http.Json;

namespace Lagersystem.Blazor.Services.Api;

public class CustomerApiService : ICustomerService
{
    private readonly HttpClient _httpClient;

    public CustomerApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    private async Task<T?> SendRequestAsync<T>(HttpRequestMessage request)
    {
        // Include credentials (cookies) in the request
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

        var response = await _httpClient.SendAsync(request);

        // Ensure API call succeeded
        await ApiResponseHandler.EnsureSuccessAsync(response);

        if (typeof(T) == typeof(HttpResponseMessage))
        {
            return (T)(object)response;
        }

        return await response.Content.ReadFromJsonAsync<T>();
    }

    public async Task<IReadOnlyList<CustomerDto>> GetCustomersAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "api/Customer");
        return await SendRequestAsync<List<CustomerDto>>(request) ?? new List<CustomerDto>();
    }

    public async Task<CustomerDto?> GetCustomerByIdAsync(Guid id)
    {

        var request = new HttpRequestMessage(HttpMethod.Get, $"api/Customer/{id}");
        return await SendRequestAsync<CustomerDto?>(request);

    }
}