using System.Net.Http.Json;
using Lagersystem.Blazor.API.Helpers;
using Lagersystem.Blazor.Models.Dtos;
using Lagersystem.Blazor.Models.Requests;
using Lagersystem.Blazor.Services.Abstractions;

namespace Lagersystem.Blazor.Services.Api;

public class ProductApiService : IProductService
{
    private readonly HttpClient _httpClient;

    public ProductApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<ProductDto>> GetProductsAsync()
    {
        // Henter alle produkter fra API'et.
        // Hvis API'et returnerer null, sender vi en tom liste videre.
        return await _httpClient.GetFromJsonAsync<List<ProductDto>>("api/Product")
               ?? new List<ProductDto>();
    }

    public async Task<ProductDto?> GetProductByIdAsync(Guid id)
    {
        // Henter ét produkt ud fra id.
        // Returnerer null hvis produktet ikke findes.
        return await _httpClient.GetFromJsonAsync<ProductDto>($"api/Product/get-product-by-id/{id}");
    }

    public async Task<IReadOnlyList<ProductDto>> GetProductsByNameAsync(string name)
    {
        // Henter produkter ud fra navn.
        // Hvis API'et returnerer null, bruges en tom liste i stedet.
        return await _httpClient.GetFromJsonAsync<List<ProductDto>>(
                   $"api/Product/get-products-by-name/{Uri.EscapeDataString(name)}")
               ?? new List<ProductDto>();
    }

    public async Task<IReadOnlyList<ProductDto>> GetProductsByDescriptionAsync(string description)
    {
        // Henter produkter ud fra beskrivelse.
        return await _httpClient.GetFromJsonAsync<List<ProductDto>>(
                   $"api/Product/get-products-by-description/{Uri.EscapeDataString(description)}")
               ?? new List<ProductDto>();
    }

    public async Task<IReadOnlyList<ProductDto>> GetProductsByUnitPriceAsync(decimal unitPrice)
    {
        // Henter produkter ud fra enhedspris.
        // InvariantCulture bruges for at undgå problemer med komma og punktum i URL.
        string unitPriceValue = unitPrice.ToString(System.Globalization.CultureInfo.InvariantCulture);

        return await _httpClient.GetFromJsonAsync<List<ProductDto>>(
                   $"api/Product/get-products-by-unit-price/{unitPriceValue}")
               ?? new List<ProductDto>();
    }

    public async Task<IReadOnlyList<ProductDto>> GetProductsBySizeAsync(int size)
    {
        // Henter produkter ud fra størrelse.
        return await _httpClient.GetFromJsonAsync<List<ProductDto>>(
                   $"api/Product/get-products-by-size/{size}")
               ?? new List<ProductDto>();
    }

    public async Task<IReadOnlyList<ProductDto>> GetProductsByWarehouseAsync(string warehouse)
    {
        // Henter produkter ud fra lager/warehouse.
        return await _httpClient.GetFromJsonAsync<List<ProductDto>>(
                   $"api/Product/get-products-by-warehouse/{Uri.EscapeDataString(warehouse)}")
               ?? new List<ProductDto>();
    }

    public async Task<IReadOnlyList<ProductDto>> GetProductsByUnitStockAsync(int unitStock)
    {
        // Henter produkter ud fra antal på lager.
        return await _httpClient.GetFromJsonAsync<List<ProductDto>>(
                   $"api/Product/get-products-by-unit-stock/{unitStock}")
               ?? new List<ProductDto>();
    }

    public async Task<IReadOnlyList<ProductDto>> GetProductsByUnitStatusAsync(int unitStatus)
    {
        // Henter produkter ud fra unit status.
        // Lige nu bruges int for at holde Blazor-koden løs koblet til API'et.
        return await _httpClient.GetFromJsonAsync<List<ProductDto>>(
                   $"api/Product/get-products-by-unit-status/{unitStatus}")
               ?? new List<ProductDto>();
    }

    public async Task CreateProductAsync(CreateProductRequest request)
    {
        // Opretter et nyt produkt via POST.
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/Product", request);

        // Tjekker om API'et accepterede requestet.
        await ApiResponseHandler.EnsureSuccessAsync(response);
    }

    public Task UpdateProductAsync(Guid id, UpdateProductRequest request)
    {
        // Placeholder:
        // API'et forventer lige nu en model hvor Id findes i body,
        // fordi controlleren sammenligner route-id med product.Id.
        //
        // UpdateProductRequest har ikke Id endnu,
        // så denne metode bør vente til request-modellen matcher API'et.
        //
        // Når den er klar, kan den implementeres med en model,
        // der indeholder Id og sendes til:
        // api/Product/{id}

        throw new NotImplementedException(
            "Product update er ikke klar endnu, fordi request-modellen ikke matcher API'et.");
    }

    public async Task DeleteProductAsync(Guid id)
    {
        // Sletter et produkt ud fra id.
        HttpResponseMessage response = await _httpClient.DeleteAsync($"api/Product/{id}");

        // Tjekker om sletningen lykkedes.
        await ApiResponseHandler.EnsureSuccessAsync(response);
    }
}