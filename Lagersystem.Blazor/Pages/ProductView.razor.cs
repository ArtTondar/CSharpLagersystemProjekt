using Lagersystem.Blazor.State;
using Microsoft.AspNetCore.Components;

namespace Lagersystem.Blazor.Pages;

public partial class ProductView
{
    [Inject]
    public ProductState ProductState { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await ProductState.LoadProductsAsync();
    }

    private void OnSearchChanged(string value)
    {
        ProductState.SearchTerm = value;
    }

    private void ClearFilters()
    {
        ProductState.ClearAllFilters();
    }
}