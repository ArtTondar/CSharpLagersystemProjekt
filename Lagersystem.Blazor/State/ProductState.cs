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
                    (product.Description ?? string.Empty).Contains(SearchTerm, StringComparison.OrdinalIgnoreCase));
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
    public async Task LoadProductsByNameAsync(string name)
    {
        IsLoading = true;

        try
        {
            Products = await _productService.GetProductsByNameAsync(name);
        }
        finally
        {
            IsLoading = false;
        }
    }

    public async Task LoadProductsByDescriptionAsync(string description)
    {
        IsLoading = true;

        try
        {
            Products = await _productService.GetProductsByDescriptionAsync(description);
        }
        finally
        {
            IsLoading = false;
        }
    }

    public async Task LoadProductsByUnitPriceAsync(decimal unitPrice)
    {
        IsLoading = true;

        try
        {
            Products = await _productService.GetProductsByUnitPriceAsync(unitPrice);
        }
        finally
        {
            IsLoading = false;
        }
    }

    public async Task LoadProductsBySizeAsync(int size)
    {
        IsLoading = true;

        try
        {
            Products = await _productService.GetProductsBySizeAsync(size);
        }
        finally
        {
            IsLoading = false;
        }
    }

    public async Task LoadProductsByWarehouseAsync(string warehouse)
    {
        IsLoading = true;

        try
        {
            Products = await _productService.GetProductsByWarehouseAsync(warehouse);
        }
        finally
        {
            IsLoading = false;
        }
    }

    public async Task LoadProductsByUnitStockAsync(int unitStock)
    {
        IsLoading = true;

        try
        {
            Products = await _productService.GetProductsByUnitStockAsync(unitStock);
        }
        finally
        {
            IsLoading = false;
        }
    }

    public async Task LoadProductsByUnitStatusAsync(int unitStatus)
    {
        IsLoading = true;

        try
        {
            Products = await _productService.GetProductsByUnitStatusAsync(unitStatus);
        }
        finally
        {
            IsLoading = false;
        }
    }
    public async Task CreateProductAsync(CreateProductRequest request)
    {
        IsLoading = true;

        try
        {
            // Opretter et nyt produkt via service-laget.
            await _productService.CreateProductAsync(request);

            // Opdaterer listen bagefter, så UI'et viser de nyeste data.
            await LoadProductsAsync();
        }
        finally
        {
            IsLoading = false;
        }
    }

    public async Task UpdateProductAsync(Guid id, UpdateProductRequest request)
    {
        IsLoading = true;

        try
        {
            // Placeholder:
            // Denne metode kalder service-laget, men service-metoden er
            // endnu ikke fuldt implementeret, fordi request-modellen ikke
            // matcher API'ets update-krav.
            await _productService.UpdateProductAsync(id, request);

            // Hvis update senere aktiveres rigtigt,
            // genindlæser vi listen bagefter.
            await LoadProductsAsync();
        }
        finally
        {
            IsLoading = false;
        }
    }

    public async Task DeleteProductAsync(Guid id)
    {
        IsLoading = true;

        try
        {
            // Sletter produktet via service-laget.
            await _productService.DeleteProductAsync(id);

            // Hvis det slettede produkt var valgt, nulstilles det.
            if (SelectedProduct?.Id == id)
            {
                SelectedProduct = null;
            }

            // Opdaterer listen bagefter.
            await LoadProductsAsync();
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