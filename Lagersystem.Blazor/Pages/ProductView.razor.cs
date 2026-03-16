using Lagersystem.Blazor.Models.Dtos;
using Lagersystem.Blazor.Models.Requests;
using Lagersystem.Blazor.State;
using Microsoft.AspNetCore.Components;

namespace Lagersystem.Blazor.Pages;

public partial class ProductView
{
    [Inject]
    public ProductState ProductState { get; set; } = default!;

    public IReadOnlyList<ProductDto> Products => ProductState.FilteredProducts;
    public bool IsLoading => ProductState.IsLoading;

    public string SearchTerm
    {
        get => ProductState.SearchTerm;
        set => ProductState.SearchTerm = value;
    }

    public string SortField
    {
        get => ProductState.SortField;
        set => ProductState.SortField = value;
    }

    public bool SortDescending
    {
        get => ProductState.SortDescending;
        set => ProductState.SortDescending = value;
    }

    private ProductFormModel Form { get; set; } = new();
    private bool IsEditMode => Form.Id.HasValue;

    protected override async Task OnInitializedAsync()
    {
        await ProductState.LoadProductsAsync();
    }

    private void ClearFilters()
    {
        ProductState.ClearAllFilters();
    }

    private void ToggleSortDirection()
    {
        SortDescending = !SortDescending;
    }

    private void StartCreate()
    {
        Form = new ProductFormModel();
        ProductState.ClearSelectedProduct();
    }

    private void StartEdit(ProductDto product)
    {
        ProductState.SelectProduct(product);

        Form = new ProductFormModel
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            UnitPrice = product.UnitPrice,
            Size = product.Size,
            Warehouse = product.Warehouse,
            UnitStock = product.UnitStock,
            UnitStatus = product.UnitStatus
        };
    }

    private async Task SaveAsync()
    {
        if (IsEditMode)
        {
            await ProductState.UpdateProductAsync(Form.Id!.Value, new UpdateProductRequest
            {
                Name = Form.Name,
                Description = Form.Description,
                UnitPrice = Form.UnitPrice,
                Size = Form.Size,
                Warehouse = Form.Warehouse,
                UnitStock = Form.UnitStock,
                UnitStatus = Form.UnitStatus
            });

            return;
        }

        await ProductState.CreateProductAsync(new CreateProductRequest
        {
            Name = Form.Name,
            Description = Form.Description,
            UnitPrice = Form.UnitPrice,
            Size = Form.Size,
            Warehouse = Form.Warehouse,
            UnitStock = Form.UnitStock,
            UnitStatus = Form.UnitStatus
        });

        StartCreate();
    }

    private async Task DeleteAsync(Guid id)
    {
        await ProductState.DeleteProductAsync(id);
        StartCreate();
    }

    private sealed class ProductFormModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int Size { get; set; }
        public string Warehouse { get; set; } = string.Empty;
        public int UnitStock { get; set; }
        public UnitStatus UnitStatus { get; set; } = UnitStatus.InStock;
    }
}