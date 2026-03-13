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

    public int? SelectedSize { get; set; }

    public string SelectedWarehouse { get; set; } = string.Empty;

    public UnitStatus? SelectedUnitStatus { get; set; }

    public IReadOnlyList<ProductDto> FilteredProducts =>
        Products
            .Where(product =>
                string.IsNullOrWhiteSpace(SearchTerm) ||
                product.Name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
            .Where(product =>
                !SelectedSize.HasValue ||
                product.Size == SelectedSize.Value)
            .Where(product =>
                string.IsNullOrWhiteSpace(SelectedWarehouse) ||
                product.Warehouse.Contains(SelectedWarehouse, StringComparison.OrdinalIgnoreCase))
            .Where(product =>
                !SelectedUnitStatus.HasValue ||
                product.UnitStatus == SelectedUnitStatus.Value)
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

    public void ClearFilters()
    {
        SelectedSize = null;
        SelectedWarehouse = string.Empty;
        SelectedUnitStatus = null;
    }

    public void ClearAllFilters()
    {
        ClearSearch();
        ClearFilters();
    }
}