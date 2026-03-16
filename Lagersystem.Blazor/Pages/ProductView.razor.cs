//using Lagersystem.Blazor.Models.Dtos;
//using Lagersystem.Blazor.State;
//using Microsoft.AspNetCore.Components;

//namespace Lagersystem.Blazor.Pages;

//public partial class ProductView
//{
//    [Inject]
//    public ProductState ProductState { get; set; } = default!;

//    public IReadOnlyList<ProductDto> Products => ProductState.FilteredProducts;

//    public bool IsLoading => ProductState.IsLoading;

//    public string SearchTerm
//    {
//        get => ProductState.SearchTerm;
//        set => ProductState.SearchTerm = value;
//    }

//    public int? SelectedSize
//    {
//        get => ProductState.SelectedSize;
//        set => ProductState.SelectedSize = value;
//    }

//    public string SelectedWarehouse
//    {
//        get => ProductState.SelectedWarehouse;
//        set => ProductState.SelectedWarehouse = value;
//    }

//    public UnitStatus? SelectedUnitStatus
//    {
//        get => ProductState.SelectedUnitStatus;
//        set => ProductState.SelectedUnitStatus = value;
//    }

//    public string SortField
//    {
//        get => ProductState.SortField;
//        set => ProductState.SortField = value;
//    }

//    public bool SortDescending
//    {
//        get => ProductState.SortDescending;
//        set => ProductState.SortDescending = value;
//    }

//    protected override async Task OnInitializedAsync()
//    {
//        await ProductState.LoadProductsAsync();
//    }

//    private void ClearFilters()
//    {
//        ProductState.ClearAllFilters();
//    }

//    private void SelectProduct(ProductDto product)
//    {
//        ProductState.SelectProduct(product);
//    }

//    private void ToggleSortDirection()
//    {
//        SortDescending = !SortDescending;
//    }
//}