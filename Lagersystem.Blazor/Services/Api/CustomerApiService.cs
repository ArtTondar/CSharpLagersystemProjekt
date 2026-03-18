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