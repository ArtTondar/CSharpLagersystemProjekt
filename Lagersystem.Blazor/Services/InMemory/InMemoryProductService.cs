using Lagersystem.Blazor.Models.Dtos;
using Lagersystem.Blazor.Models.Requests;
using Lagersystem.Blazor.Services.Abstractions;

namespace Lagersystem.Blazor.Services.InMemory;

public class InMemoryProductService : IProductService
{
    private static readonly List<ProductDto> _products =
    [
        new ProductDto
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Name = "T-Shirt Basic",
            Description = "Sort basic t-shirt",
            UnitPrice = 149.95m,
            Size = 42,
            Warehouse = "Aarhus",
            UnitStock = 25,
            UnitStatus = UnitStatus.InStock
        },
        new ProductDto
        {
            Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            Name = "Sneakers Street",
            Description = "Hvide sneakers",
            UnitPrice = 699.00m,
            Size = 43,
            Warehouse = "Silkeborg",
            UnitStock = 12,
            UnitStatus = UnitStatus.InStock
        },
        new ProductDto
        {
            Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
            Name = "Jacket Winter",
            Description = "Vinterjakke med foer",
            UnitPrice = 1199.50m,
            Size = 44,
            Warehouse = "Aarhus",
            UnitStock = 4,
            UnitStatus = UnitStatus.Reserved
        }
    ];

    public Task<IReadOnlyList<ProductDto>> GetProductsAsync()
    {
        return Task.FromResult<IReadOnlyList<ProductDto>>(_products.ToList());
    }

    public Task<ProductDto?> GetProductByIdAsync(Guid id)
    {
        ProductDto? product = _products.FirstOrDefault(p => p.Id == id);
        return Task.FromResult(product);
    }

    public Task CreateProductAsync(CreateProductRequest request)
    {
        var product = new ProductDto
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            UnitPrice = request.UnitPrice,
            Size = request.Size,
            Warehouse = request.Warehouse,
            UnitStock = request.UnitStock,
            UnitStatus = request.UnitStatus
        };

        _products.Add(product);
        return Task.CompletedTask;
    }

    public Task UpdateProductAsync(Guid id, UpdateProductRequest request)
    {
        ProductDto? product = _products.FirstOrDefault(p => p.Id == id);

        if (product is null)
        {
            return Task.CompletedTask;
        }

        product.Name = request.Name;
        product.Description = request.Description;
        product.UnitPrice = request.UnitPrice;
        product.Size = request.Size;
        product.Warehouse = request.Warehouse;
        product.UnitStock = request.UnitStock;
        product.UnitStatus = request.UnitStatus;

        return Task.CompletedTask;
    }

    public Task DeleteProductAsync(Guid id)
    {
        ProductDto? product = _products.FirstOrDefault(p => p.Id == id);

        if (product is null)
        {
            return Task.CompletedTask;
        }

        _products.Remove(product);
        return Task.CompletedTask;
    }
}