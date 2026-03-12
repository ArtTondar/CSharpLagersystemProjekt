using Lagersystem.Blazor.Models.Dtos;
using Lagersystem.Blazor.Models.Requests;


namespace Lagersystem.Blazor.Services.Abstractions
{
    public interface IProductService
    {
        Task<IReadOnlyList<ProductDto>> GetProductsAsync();
        Task<ProductDto> GetProductByIdAsync(Guid id);
        Task CreateProductAsync(CreateProductRequest request);
        Task UpdateProductAsync(Guid id, UpdateProductRequest request);
        Task DeleteProductAsync(Guid id);
    }
}
