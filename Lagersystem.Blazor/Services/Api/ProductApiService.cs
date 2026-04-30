using Lagersystem.Blazor.API.Helpers;
using Lagersystem.Blazor.Models.Dtos;
using Lagersystem.Blazor.Models.Requests;
using Lagersystem.Blazor.Services.Abstractions;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using System.Globalization;
using System.Net.Http.Json;

namespace Lagersystem.Blazor.Services.Api;

public class ProductApiService : IProductService
{
    private readonly HttpClient _httpClient;

    public ProductApiService(HttpClient httpClient)
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
            return (T)(object)response;

        return await response.Content.ReadFromJsonAsync<T>();
    }

    public async Task<IReadOnlyList<ProductDto>> GetProductsAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "api/Product");
        return await SendRequestAsync<List<ProductDto>>(request) ?? new List<ProductDto>();
    }

    public async Task<ProductDto?> GetProductByIdAsync(Guid id)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"api/Product/get-product-by-id/{id}");
        var response = await _httpClient.SendAsync(request);

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            return null;

        await ApiResponseHandler.EnsureSuccessAsync(response);
        return await response.Content.ReadFromJsonAsync<ProductDto>();
    }

    public async Task<IReadOnlyList<ProductDto>> GetProductsByNameAsync(string name)
    {
        var request = new HttpRequestMessage(HttpMethod.Get,
            $"api/Product/get-products-by-name/{Uri.EscapeDataString(name)}");
        return await SendRequestAsync<List<ProductDto>>(request) ?? new List<ProductDto>();
    }

    public async Task<IReadOnlyList<ProductDto>> GetProductsByDescriptionAsync(string description)
    {
        var request = new HttpRequestMessage(HttpMethod.Get,
            $"api/Product/get-products-by-description/{Uri.EscapeDataString(description)}");
        return await SendRequestAsync<List<ProductDto>>(request) ?? new List<ProductDto>();
    }

    public async Task<IReadOnlyList<ProductDto>> GetProductsByUnitPriceAsync(decimal unitPrice)
    {
        string unitPriceValue = unitPrice.ToString(CultureInfo.InvariantCulture);
        var request = new HttpRequestMessage(HttpMethod.Get,
            $"api/Product/get-products-by-unit-price/{unitPriceValue}");
        return await SendRequestAsync<List<ProductDto>>(request) ?? new List<ProductDto>();
    }

    public async Task<IReadOnlyList<ProductDto>> GetProductsBySizeAsync(int size)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"api/Product/get-products-by-size/{size}");
        return await SendRequestAsync<List<ProductDto>>(request) ?? new List<ProductDto>();
    }

    public async Task<IReadOnlyList<ProductDto>> GetProductsByWarehouseAsync(string warehouse)
    {
        var request = new HttpRequestMessage(HttpMethod.Get,
            $"api/Product/get-products-by-warehouse/{Uri.EscapeDataString(warehouse)}");
        return await SendRequestAsync<List<ProductDto>>(request) ?? new List<ProductDto>();
    }

    public async Task<IReadOnlyList<ProductDto>> GetProductsByUnitStockAsync(int unitStock)
    {
        var request = new HttpRequestMessage(HttpMethod.Get,
            $"api/Product/get-products-by-unit-stock/{unitStock}");
        return await SendRequestAsync<List<ProductDto>>(request) ?? new List<ProductDto>();
    }

    public async Task<IReadOnlyList<ProductDto>> GetProductsByUnitStatusAsync(int unitStatus)
    {
        var request = new HttpRequestMessage(HttpMethod.Get,
            $"api/Product/get-products-by-unit-status/{unitStatus}");
        return await SendRequestAsync<List<ProductDto>>(request) ?? new List<ProductDto>();
    }

    public async Task<ProductDto> CreateProductAsync(CreateProductRequest requestData)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "api/Product")
        {
            Content = JsonContent.Create(requestData)
        };

        var createdProduct = await SendRequestAsync<ProductDto?>(request);

        if (createdProduct is null)
            throw new InvalidOperationException("API'et returnerede ikke et oprettet produkt.");

        return createdProduct;
    }

    public async Task UpdateProductAsync(Guid id, UpdateProductRequest requestData)
    {
        ArgumentNullException.ThrowIfNull(requestData);
        requestData.Id = id;

        var request = new HttpRequestMessage(HttpMethod.Put, $"api/Product/{id}")
        {
            Content = JsonContent.Create(requestData)
        };

        await SendRequestAsync<HttpResponseMessage>(request);
    }

    public async Task DeleteProductAsync(Guid id)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, $"api/Product/{id}");
        await SendRequestAsync<HttpResponseMessage>(request);
    }
}