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
            // Henter et enkelt produkt fra service-laget
            // og gemmer det som valgt produkt i state.
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

    // --------------------------------------------------------
    // UPDATE PRODUCT
    // --------------------------------------------------------
    public async Task UpdateProductAsync(Guid id, UpdateProductRequest request)
    {
        // Guard clause:
        // Request må ikke være null.
        ArgumentNullException.ThrowIfNull(request);

        IsLoading = true;

        try
        {
            // Kalder service-laget, som sender PUT-request til API'et.
            await _productService.UpdateProductAsync(id, request);

            // Opretter en ny DTO med de opdaterede værdier,
            // så state afspejler ændringen uden at kræve en fuld reload.
            ProductDto updatedProduct = new()
            {
                Id = id,
                Name = request.Name,
                Description = request.Description,
                UnitPrice = request.UnitPrice,
                Size = request.Size,
                Warehouse = request.Warehouse,
                UnitStock = request.UnitStock,
                UnitStatus = request.UnitStatus
            };

            // Kopierer den eksisterende liste til en ny liste,
            // så state opdateres på en kontrolleret måde.
            List<ProductDto> updatedProducts = Products.ToList();

            int index = updatedProducts.FindIndex(product => product.Id == id);

            // Hvis produktet findes i listen, erstattes det med den nye version.
            if (index >= 0)
            {
                updatedProducts[index] = updatedProduct;
                Products = updatedProducts;
            }

            // Hvis det valgte produkt er det samme som det opdaterede,
            // opdateres SelectedProduct også.
            if (SelectedProduct?.Id == id)
            {
                SelectedProduct = updatedProduct;
            }
        }
        finally
        {
            IsLoading = false;
        }
    }

    // --------------------------------------------------------
    // DELETE PRODUCT
    // --------------------------------------------------------
    public async Task DeleteProductAsync(Guid id)
    {
        IsLoading = true;

        try
        {
            // Kalder service-laget, som sender DELETE-request til API'et.
            await _productService.DeleteProductAsync(id);

            // Fjerner det slettede produkt fra den lokale liste,
            // så UI opdateres uden at hente alle produkter igen.
            Products = Products
                .Where(product => product.Id != id)
                .ToList();

            // Nulstiller valgt produkt,
            // hvis det slettede produkt var valgt.
            if (SelectedProduct?.Id == id)
            {
                SelectedProduct = null;
            }
        }
        finally
        {
            IsLoading = false;
        }
    }
}