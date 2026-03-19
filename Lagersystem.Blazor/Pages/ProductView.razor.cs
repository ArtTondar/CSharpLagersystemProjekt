using Lagersystem.Blazor.Models.Dtos;
using Lagersystem.Blazor.State;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Lagersystem.Blazor.Pages;

public partial class ProductView
{
    [Inject]
    public ProductState ProductState { get; set; } = default!;

    [Inject]
    public NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    public IJSRuntime JS { get; set; } = default!;

    [Inject]
    public AuthState AuthState { get; set; } = default!;

    public IReadOnlyList<ProductDto> Products => ProductState.Products;

    public ProductDto? SelectedProduct => ProductState.SelectedProduct;

    public bool IsLoading => ProductState.IsLoading;

    public string ErrorMessage { get; set; } = string.Empty;

    public bool HasError => !string.IsNullOrWhiteSpace(ErrorMessage);

    public bool ShowEmptyMessage => !IsLoading && Products.Count == 0;

    public int ProductTableColumnCount => AuthState.IsAdmin ? 10 : 8;

    protected override async Task OnInitializedAsync()
    {
        await LoadProductsAsync();
    }

    public async Task ReloadProductsAsync()
    {
        await LoadProductsAsync();
    }

    private async Task LoadProductsAsync()
    {
        ClearError();

        try
        {
            await ProductState.LoadProductsAsync();
        }
        catch (Exception ex)
        {
            SetError($"Fejl ved hentning af produkter: {ex.Message}");
        }
    }

    public void OpenProduct(Guid productId)
    {
        if (!AuthState.IsAdmin)
        {
            return;
        }

        NavigationManager.NavigateTo($"/products/{productId}");
    }

    public void OpenCreateProductPage()
    {
        if (!AuthState.IsAdmin)
        {
            return;
        }

        NavigationManager.NavigateTo("/products/create");
    }

    public async Task DeleteProductAsync(Guid productId)
    {
        if (!AuthState.IsAdmin)
        {
            return;
        }

        ClearError();

        try
        {
            bool confirmed = await JS.InvokeAsync<bool>(
                "confirm",
                "Er du sikker på, at du vil slette dette produkt?");

            if (!confirmed)
            {
                return;
            }

            await ProductState.DeleteProductAsync(productId);
        }
        catch (Exception ex)
        {
            SetError($"Fejl ved sletning af produkt: {ex.Message}");
        }
    }

    private void ClearError()
    {
        ErrorMessage = string.Empty;
    }

    private void SetError(string message)
    {
        ErrorMessage = message;
    }
}