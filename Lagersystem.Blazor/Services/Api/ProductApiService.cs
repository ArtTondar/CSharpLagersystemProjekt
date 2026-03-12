using System.Net.Http.Json;
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
        return await _httpClient.GetFromJsonAsync<List<ProductDto>>("api/Product")
               ?? new List<ProductDto>();
    }

    public async Task<ProductDto?> GetProductByIdAsync(Guid id)
    {
        return await _httpClient.GetFromJsonAsync<ProductDto>($"api/Product/get-product-by-id/{id}");
    }

    public async Task CreateProductAsync(CreateProductRequest request)
    {
        await _httpClient.PostAsJsonAsync("api/Product", request);
    }

    public async Task UpdateProductAsync(Guid id, UpdateProductRequest request)
    {
        await _httpClient.PutAsJsonAsync($"api/Product?id={id}", request);
    }

    public async Task DeleteProductAsync(Guid id)
    {
        await _httpClient.DeleteAsync($"api/Product/{id}");
    }
}