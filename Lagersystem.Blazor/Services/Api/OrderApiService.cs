using System.Globalization;
using System.Net.Http.Json;
using Lagersystem.Blazor.API.Helpers;
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

    public async Task<IReadOnlyList<OrderDto>> GetOrdersAsync()
    {
        // Henter alle ordrer fra API'et.
        // Hvis API'et returnerer null, sender vi en tom liste videre,
        // så UI'et ikke skal håndtere null.
        return await _httpClient.GetFromJsonAsync<List<OrderDto>>("api/Order")
               ?? new List<OrderDto>();
    }

    public async Task<OrderDto?> GetOrderByIdAsync(Guid id)
    {
        // Henter én ordre ud fra id.
        // Returnerer null hvis ordren ikke findes,
        // eller hvis API'et sender tomt svar tilbage.
        return await _httpClient.GetFromJsonAsync<OrderDto>($"api/Order/get-order-by-id/{id}");
    }

    public async Task<IReadOnlyList<OrderDto>> GetOrdersByCustomerIdAsync(Guid customerId)
    {
        // Henter ordrer ud fra kunde-id.
        // Hvis der ikke findes nogen, returneres en tom liste.
        return await _httpClient.GetFromJsonAsync<List<OrderDto>>(
                   $"api/Order/get-orders-by-customer-id/{customerId}")
               ?? new List<OrderDto>();
    }

    public async Task<IReadOnlyList<OrderDto>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        // Henter ordrer inden for et datointerval.
        // Controlleren forventer query string og ikke route-parametre,
        // så datoerne sendes som ?startDate=...&endDate=...
        string startDateValue = Uri.EscapeDataString(startDate.ToString("O"));
        string endDateValue = Uri.EscapeDataString(endDate.ToString("O"));

        string url = $"api/Order/get-orders-by-date?startDate={startDateValue}&endDate={endDateValue}";

        return await _httpClient.GetFromJsonAsync<List<OrderDto>>(url)
               ?? new List<OrderDto>();
    }

    public async Task<IReadOnlyList<OrderDto>> GetOrdersByTotalPriceAsync(decimal totalPrice)
    {
        // Henter ordrer ud fra totalpris.
        // InvariantCulture bruges for at undgå problemer med komma og punktum.
        string totalPriceValue = totalPrice.ToString(CultureInfo.InvariantCulture);

        return await _httpClient.GetFromJsonAsync<List<OrderDto>>(
                   $"api/Order/get-orders-by-total-price/{totalPriceValue}")
               ?? new List<OrderDto>();
    }

    public async Task CreateOrderAsync(CreateOrderRequest request)
    {
        // Opretter en ny ordre via POST.
        // Request-objektet bliver sendt som JSON til API'et.
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/Order", request);

        // Tjekker om API-kaldet lykkedes.
        // Hvis ikke, bliver der kastet en tydelig fejl.
        await ApiResponseHandler.EnsureSuccessAsync(response);
    }

    public Task UpdateOrderAsync(Guid id, UpdateOrderRequest request)
    {
        // Placeholder:
        // Denne metode er bevidst ikke aktiveret endnu.
        //
        // Hvorfor:
        // 1. API'et forventer en Order-model med Id i body.
        // 2. UpdateOrderRequest har ikke Id lige nu.
        // 3. API-routen i backend er også skrevet som "/{id}",
        //    hvilket gør den usikker at ramme fra Blazor.
        //
        // Derfor lader vi den stå som en tydelig placeholder,
        // så projektet stadig bygger uden skjulte runtime-fejl.

        throw new NotImplementedException(
            "UpdateOrderAsync er ikke aktiveret endnu, fordi Blazor-requesten ikke matcher API'et.");
    }

    public Task DeleteOrderAsync(Guid id)
    {
        // Placeholder:
        // Delete findes i API'et, men backend-routen er skrevet som "/{id}".
        // Da vi ikke må ændre API'et, lader vi denne stå som placeholder indtil videre.
        //
        // Det er bedre end at aktivere noget, som måske fejler uforudsigeligt.

        throw new NotImplementedException(
            "DeleteOrderAsync er ikke aktiveret endnu, fordi API-routen ikke er klar til sikker brug fra Blazor.");
    }
}