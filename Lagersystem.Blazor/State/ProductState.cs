using Lagersystem.Blazor.Models.Dtos;
using Lagersystem.Blazor.Services.Abstractions;

namespace Lagersystem.Blazor.State;

public class ProductState
{
    private readonly IProductService _productService;

    public IReadOnlyList<ProductDto> Products { get; private set; } = new List<ProductDto>();

    public ProductDto? SelectedProduct { get; private set; }

    public bool IsLoading { get; private set; }

    public string SearchTerm { get; set; } = string.Empty;

    public IReadOnlyList<ProductDto> FilteredProducts =>
        string.IsNullOrWhiteSpace(SearchTerm)
            ? Products
            : Products
                .Where(product => product.Name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
                .ToList();

    public ProductState(IProductService productService)
    {
        _productService = productService;
    }

    public async Task LoadProductsAsync()
    {
        IsLoading = true;

        try
        {
            Products = await _productService.GetProductsAsync();
        }
        finally
        {
            IsLoading = false;
        }
    }

    public async Task LoadProductByIdAsync(Guid id)
    {
        IsLoading = true;

        try
        {
            SelectedProduct = await _productService.GetProductByIdAsync(id);
        }
        finally
        {
            IsLoading = false;
        }
    }

    public void ClearSelectedProduct()
    {
        SelectedProduct = null;
    }

    public void ClearSearch()
    {
        SearchTerm = string.Empty;
    }
}