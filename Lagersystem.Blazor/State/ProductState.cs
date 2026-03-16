using Lagersystem.Blazor.Models.Dtos;
using Lagersystem.Blazor.Models.Requests;
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
    public string SortField { get; set; } = "Name";
    public bool SortDescending { get; set; }

    public IReadOnlyList<ProductDto> FilteredProducts
    {
        get
        {
            IEnumerable<ProductDto> result = Products;

            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                result = result.Where(product =>
                    product.Name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    product.Description.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase));
            }

            if (SelectedSize.HasValue)
            {
                result = result.Where(product => product.Size == SelectedSize.Value);
            }

            if (!string.IsNullOrWhiteSpace(SelectedWarehouse))
            {
                result = result.Where(product =>
                    product.Warehouse.Contains(SelectedWarehouse, StringComparison.OrdinalIgnoreCase));
            }

            if (SelectedUnitStatus.HasValue)
            {
                result = result.Where(product => product.UnitStatus == SelectedUnitStatus.Value);
            }

            result = SortField switch
            {
                "Description" => SortDescending ? result.OrderByDescending(p => p.Description) : result.OrderBy(p => p.Description),
                "UnitPrice" => SortDescending ? result.OrderByDescending(p => p.UnitPrice) : result.OrderBy(p => p.UnitPrice),
                "Size" => SortDescending ? result.OrderByDescending(p => p.Size) : result.OrderBy(p => p.Size),
                "Warehouse" => SortDescending ? result.OrderByDescending(p => p.Warehouse) : result.OrderBy(p => p.Warehouse),
                "UnitStock" => SortDescending ? result.OrderByDescending(p => p.UnitStock) : result.OrderBy(p => p.UnitStock),
                "UnitStatus" => SortDescending ? result.OrderByDescending(p => p.UnitStatus) : result.OrderBy(p => p.UnitStatus),
                _ => SortDescending ? result.OrderByDescending(p => p.Name) : result.OrderBy(p => p.Name)
            };

            return result.ToList();
        }
    }

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

    public async Task CreateProductAsync(CreateProductRequest request)
    {
        await _productService.CreateProductAsync(request);
        await LoadProductsAsync();
    }

    public async Task UpdateProductAsync(Guid id, UpdateProductRequest request)
    {
        await _productService.UpdateProductAsync(id, request);
        await LoadProductsAsync();
        await LoadProductByIdAsync(id);
    }

    public async Task DeleteProductAsync(Guid id)
    {
        await _productService.DeleteProductAsync(id);
        SelectedProduct = null;
        await LoadProductsAsync();
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

    public void SelectProduct(ProductDto product)
    {
        SelectedProduct = product;
    }

    public void ClearSelectedProduct()
    {
        SelectedProduct = null;
    }

    public void ClearAllFilters()
    {
        SearchTerm = string.Empty;
        SelectedSize = null;
        SelectedWarehouse = string.Empty;
        SelectedUnitStatus = null;
        SortField = "Name";
        SortDescending = false;
    }
}