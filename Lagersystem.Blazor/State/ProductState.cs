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
                "Description" => SortDescending
                    ? result.OrderByDescending(product => product.Description)
                    : result.OrderBy(product => product.Description),

                "UnitPrice" => SortDescending
                    ? result.OrderByDescending(product => product.UnitPrice)
                    : result.OrderBy(product => product.UnitPrice),

                "Size" => SortDescending
                    ? result.OrderByDescending(product => product.Size)
                    : result.OrderBy(product => product.Size),

                "Warehouse" => SortDescending
                    ? result.OrderByDescending(product => product.Warehouse)
                    : result.OrderBy(product => product.Warehouse),

                "UnitStock" => SortDescending
                    ? result.OrderByDescending(product => product.UnitStock)
                    : result.OrderBy(product => product.UnitStock),

                "UnitStatus" => SortDescending
                    ? result.OrderByDescending(product => product.UnitStatus)
                    : result.OrderBy(product => product.UnitStatus),

                _ => SortDescending
                    ? result.OrderByDescending(product => product.Name)
                    : result.OrderBy(product => product.Name)
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