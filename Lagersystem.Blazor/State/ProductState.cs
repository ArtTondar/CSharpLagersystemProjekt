using Lagersystem.Blazor.Models.Dtos;
using Lagersystem.Blazor.Services.Abstractions;

namespace Lagersystem.Blazor.State;

public class ProductState
{
    private readonly IProductService _productService;

    public IReadOnlyList<ProductDto> Products { get; private set; } = new List<ProductDto>();

    public ProductDto? SelectedProduct { get; private set; }

    public bool IsLoading { get; private set; }

    public ProductState(IProductService productService)
    {
        _productService = productService;
    }

    // --------------------------------------------------------
    // CENTRAL LOADER
    // --------------------------------------------------------
    // Samler loading logik ét sted
    // så vi undgår gentaget kode i alle metoder
    private async Task ExecuteProductLoad(
        Func<Task<IReadOnlyList<ProductDto>>> loader)
    {
        IsLoading = true;

        try
        {
            Products = await loader();
        }
        finally
        {
            IsLoading = false;
        }
    }

    // --------------------------------------------------------
    // GET ALL
    // --------------------------------------------------------
    public async Task LoadProductsAsync()
    {
        await ExecuteProductLoad(() =>
            _productService.GetProductsAsync());
    }

    // --------------------------------------------------------
    // SEARCH METHODS
    // --------------------------------------------------------

    public async Task LoadProductsByNameAsync(string name)
    {
        await ExecuteProductLoad(() =>
            _productService.GetProductsByNameAsync(name));
    }

    public async Task LoadProductsByDescriptionAsync(string description)
    {
        await ExecuteProductLoad(() =>
            _productService.GetProductsByDescriptionAsync(description));
    }

    public async Task LoadProductsBySizeAsync(int size)
    {
        await ExecuteProductLoad(() =>
            _productService.GetProductsBySizeAsync(size));
    }

    public async Task LoadProductsByWarehouseAsync(string warehouse)
    {
        await ExecuteProductLoad(() =>
            _productService.GetProductsByWarehouseAsync(warehouse));
    }

    public async Task LoadProductsByUnitPriceAsync(decimal price)
    {
        await ExecuteProductLoad(() =>
            _productService.GetProductsByUnitPriceAsync(price));
    }

    public async Task LoadProductsByUnitStockAsync(int stock)
    {
        await ExecuteProductLoad(() =>
            _productService.GetProductsByUnitStockAsync(stock));
    }

    public async Task LoadProductsByUnitStatusAsync(int status)
    {
        await ExecuteProductLoad(() =>
            _productService.GetProductsByUnitStatusAsync(status));
    }

    // --------------------------------------------------------
    // SINGLE PRODUCT
    // --------------------------------------------------------

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
}