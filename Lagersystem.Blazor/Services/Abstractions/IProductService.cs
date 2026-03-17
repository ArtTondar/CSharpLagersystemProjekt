using Lagersystem.Blazor.Models.Dtos;
using Lagersystem.Blazor.Models.Requests;

namespace Lagersystem.Blazor.Services.Abstractions;

public interface IProductService
{
    Task<IReadOnlyList<ProductDto>> GetProductsAsync();

    Task<ProductDto?> GetProductByIdAsync(Guid id);

    Task<IReadOnlyList<ProductDto>> GetProductsByNameAsync(string name);

    Task<IReadOnlyList<ProductDto>> GetProductsByDescriptionAsync(string description);

    Task<IReadOnlyList<ProductDto>> GetProductsByUnitPriceAsync(decimal unitPrice);

    Task<IReadOnlyList<ProductDto>> GetProductsBySizeAsync(int size);

    Task<IReadOnlyList<ProductDto>> GetProductsByWarehouseAsync(string warehouse);

    Task<IReadOnlyList<ProductDto>> GetProductsByUnitStockAsync(int unitStock);

    Task<IReadOnlyList<ProductDto>> GetProductsByUnitStatusAsync(int unitStatus);

    Task<ProductDto> CreateProductAsync(CreateProductRequest request);

    Task UpdateProductAsync(Guid id, UpdateProductRequest request);

    Task DeleteProductAsync(Guid id);
}