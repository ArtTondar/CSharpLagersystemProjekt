//using System.Globalization;
//using System.Net.Http.Json;
//using Lagersystem.Blazor.API.Helpers;
//using Lagersystem.Blazor.Models.Dtos;
//using Lagersystem.Blazor.Models.Requests;
//using Lagersystem.Blazor.Services.Abstractions;

//namespace Lagersystem.Blazor.Services.Api;

//public class OrderApiService : IOrderService
//{
//    private readonly HttpClient _httpClient;

//    public OrderApiService(HttpClient httpClient)
//    {
//        _httpClient = httpClient;
//    }

//    public async Task<IReadOnlyList<OrderDto>> GetOrdersAsync()
//    {
//        // Henter alle ordrer fra API'et.
//        // Hvis API'et returnerer null, sender vi en tom liste videre,
//        // så UI'et ikke skal håndtere null.
//        return await _httpClient.GetFromJsonAsync<List<OrderDto>>("api/Order")
//               ?? new List<OrderDto>();
//    }

//    public async Task<OrderDto?> GetOrderByIdAsync(Guid id)
//    {
//        // Henter én ordre ud fra id.
//        // Returnerer null hvis ordren ikke findes,
//        // eller hvis API'et sender tomt svar tilbage.
//        return await _httpClient.GetFromJsonAsync<OrderDto>($"api/Order/get-order-by-id/{id}");
//    }

//    public async Task<IReadOnlyList<OrderDto>> GetOrdersByCustomerIdAsync(Guid customerId)
//    {
//        // Henter ordrer ud fra kunde-id.
//        // Hvis der ikke findes nogen, returneres en tom liste.
//        return await _httpClient.GetFromJsonAsync<List<OrderDto>>(
//                   $"api/Order/get-orders-by-customer-id/{customerId}")
//               ?? new List<OrderDto>();
//    }

//    public async Task<IReadOnlyList<OrderDto>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate)
//    {
//        // Henter ordrer inden for et datointerval.
//        // Controlleren forventer query string og ikke route-parametre,
//        // så datoerne sendes som ?startDate=...&endDate=...
//        string startDateValue = Uri.EscapeDataString(startDate.ToString("O"));
//        string endDateValue = Uri.EscapeDataString(endDate.ToString("O"));

//        string url = $"api/Order/get-orders-by-date?startDate={startDateValue}&endDate={endDateValue}";

//        return await _httpClient.GetFromJsonAsync<List<OrderDto>>(url)
//               ?? new List<OrderDto>();
//    }

//    public async Task<IReadOnlyList<OrderDto>> GetOrdersByTotalPriceAsync(decimal totalPrice)
//    {
//        // Henter ordrer ud fra totalpris.
//        // InvariantCulture bruges for at undgå problemer med komma og punktum.
//        string totalPriceValue = totalPrice.ToString(CultureInfo.InvariantCulture);

//        return await _httpClient.GetFromJsonAsync<List<OrderDto>>(
//                   $"api/Order/get-orders-by-total-price/{totalPriceValue}")
//               ?? new List<OrderDto>();
//    }

//    public async Task CreateOrderAsync(CreateOrderRequest request)
//    {
//        // Opretter en ny ordre via POST.
//        // Request-objektet bliver sendt som JSON til API'et.
//        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/Order", request);

//        // Tjekker om API-kaldet lykkedes.
//        // Hvis ikke, bliver der kastet en tydelig fejl.
//        await ApiResponseHandler.EnsureSuccessAsync(response);
//    }

//    public async Task UpdateOrderAsync(Guid id, UpdateOrderRequest request)
//    {
//        // Opdaterer en eksisterende ordre via PUT.
//        // Id sendes både i route og i request body,
//        // fordi API'et sammenligner route-id med order.Id.
//        HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"api/Order/{id}", request);

//        // Sikrer at API-kaldet lykkedes.
//        // Hvis ikke, kastes en tydelig fejl.
//        await ApiResponseHandler.EnsureSuccessAsync(response);
//    }

//    public async Task DeleteOrderAsync(Guid id)
//    {
//        // Sletter en ordre ud fra id.
//        // Endpointet forventer id som route-parameter.
//        HttpResponseMessage response = await _httpClient.DeleteAsync($"api/Order/{id}");

//        // Sikrer at API-kaldet lykkedes.
//        // Hvis ikke, kastes en tydelig fejl.
//        await ApiResponseHandler.EnsureSuccessAsync(response);
//    }
//}

using Lagersystem.Blazor.API.Helpers;
using Lagersystem.Blazor.Models.Dtos;
using Lagersystem.Blazor.Models.Requests;
using Lagersystem.Blazor.Services.Abstractions;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using System.Globalization;
using System.Net.Http.Json;

namespace Lagersystem.Blazor.Services.Api;

public class OrderApiService : IOrderService
{
    private readonly HttpClient _httpClient;

    public OrderApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    private async Task<T?> SendRequestAsync<T>(HttpRequestMessage request)
    {
        // Include credentials (cookies) in the browser request
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

        var response = await _httpClient.SendAsync(request);
        await ApiResponseHandler.EnsureSuccessAsync(response);

        if (typeof(T) == typeof(HttpResponseMessage))
        {
            return (T)(object)response;
        }

        return await response.Content.ReadFromJsonAsync<T>();
    }

    public async Task<IReadOnlyList<OrderDto>> GetOrdersAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "api/Order");
        return await SendRequestAsync<List<OrderDto>>(request) ?? new List<OrderDto>();
    }

    public async Task<OrderDto?> GetOrderByIdAsync(Guid id)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"api/Order/get-order-by-id/{id}");
        return await SendRequestAsync<OrderDto?>(request);
    }

    public async Task<IReadOnlyList<OrderDto>> GetOrdersByCustomerIdAsync(Guid customerId)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"api/Order/get-orders-by-customer-id/{customerId}");
        return await SendRequestAsync<List<OrderDto>>(request) ?? new List<OrderDto>();
    }

    public async Task<IReadOnlyList<OrderDto>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        string startDateValue = Uri.EscapeDataString(startDate.ToString("O"));
        string endDateValue = Uri.EscapeDataString(endDate.ToString("O"));
        string url = $"api/Order/get-orders-by-date?startDate={startDateValue}&endDate={endDateValue}";

        var request = new HttpRequestMessage(HttpMethod.Get, url);
        return await SendRequestAsync<List<OrderDto>>(request) ?? new List<OrderDto>();
    }

    public async Task<IReadOnlyList<OrderDto>> GetOrdersByTotalPriceAsync(decimal totalPrice)
    {
        string totalPriceValue = totalPrice.ToString(CultureInfo.InvariantCulture);
        var request = new HttpRequestMessage(HttpMethod.Get, $"api/Order/get-orders-by-total-price/{totalPriceValue}");
        return await SendRequestAsync<List<OrderDto>>(request) ?? new List<OrderDto>();
    }


    public async Task<OrderDto> CreateOrderAsync(CreateOrderRequest requestData)

    {
        var request = new HttpRequestMessage(HttpMethod.Post, "api/Order")
        {
            Content = JsonContent.Create(requestData)
        };


 // Send the request and get the created order
    OrderDto? createdOrder = await SendRequestAsync<OrderDto?>(request);

    // Stop if API didn't return an order as expected
    if (createdOrder is null)
    {
        throw new InvalidOperationException("API'et returnerede ikke en oprettet ordre.");
    }

    return createdOrder;
}

    public async Task UpdateOrderAsync(Guid id, UpdateOrderRequest requestData)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, $"api/Order/{id}")
        {
            Content = JsonContent.Create(requestData)
        };

        await SendRequestAsync<HttpResponseMessage>(request);
    }

    public async Task DeleteOrderAsync(Guid id)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, $"api/Order/{id}");
        await SendRequestAsync<HttpResponseMessage>(request);
    }
}