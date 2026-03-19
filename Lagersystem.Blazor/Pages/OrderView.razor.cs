using Lagersystem.Blazor.Models.Dtos;
using Lagersystem.Blazor.Models.Requests;
using Lagersystem.Blazor.Models.ViewModels;
using Lagersystem.Blazor.State;
using Lagersystem.Blazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace Lagersystem.Blazor.Pages;

public partial class OrderView
{
    [Inject]
    public OrderState OrderState { get; set; } = default!;

    [Inject]
    public CustomerState CustomerState { get; set; } = default!;

    [Inject]
    public ProductState ProductState { get; set; } = default!;

    [Inject]
    public NavigationManager NavigationManager { get; set; } = default!;

    public IReadOnlyList<OrderDto> Orders => OrderState.Orders;

    public IReadOnlyList<CustomerDto> Customers => CustomerState.Customers;

    public IReadOnlyList<ProductDto> Products => ProductState.Products;

    public OrderDto? SelectedOrder => OrderState.SelectedOrder;

    public bool IsLoading => OrderState.IsLoading || CustomerState.IsLoading || ProductState.IsLoading;

    public string PageErrorMessage { get; set; } = string.Empty;

    public string ModalErrorMessage { get; set; } = string.Empty;

    public bool HasPageError => !string.IsNullOrWhiteSpace(PageErrorMessage);

    public bool HasModalError => !string.IsNullOrWhiteSpace(ModalErrorMessage);

    public bool ShowEmptyMessage => !IsLoading && Orders.Count == 0;

    public bool IsEditModalOpen { get; set; }

    public EditableOrderViewModel? EditableOrder { get; set; }
    [Inject]
    public AuthState AuthState { get; set; } = default!;

    private Guid _newProductId;

    public Guid NewProductId
    {
        get => _newProductId;
        set
        {
            _newProductId = value;
            UpdateNewUnitPriceFromSelectedProduct();
        }
    }

    public int NewQuantity { get; set; } = 1;

    public decimal NewUnitPrice { get; set; }

    public IReadOnlyList<GroupedOrderDetailViewModel> GroupedOrderDetails
    {
        get
        {
            if (EditableOrder is null || EditableOrder.OrderDetails.Count == 0)
            {
                return new List<GroupedOrderDetailViewModel>();
            }

            if (HasDuplicateProductsWithDifferentPrice())
            {
                return new List<GroupedOrderDetailViewModel>();
            }

            return OrderDetailGroupingHelper.GroupOrderDetails(
                EditableOrder.OrderDetails,
                GetProductDisplayName);
        }
    }

    protected override async Task OnInitializedAsync()
    {
        if (!AuthState.IsAdmin)
        {
            NavigationManager.NavigateTo("/");
            return;
        }

        await LoadPageDataAsync();
    }
    public async Task ReloadOrdersAsync()
    {
        await LoadPageDataAsync();
    }

    private async Task LoadPageDataAsync()
    {
        ClearPageError();

        try
        {
            await OrderState.LoadOrdersAsync();
            await CustomerState.LoadCustomersAsync();
            await ProductState.LoadProductsAsync();
        }
        catch (Exception ex)
        {
            SetPageError($"Fejl ved hentning af data:");
        }
    }

    public void OpenCreateOrderPage()
    {
        NavigationManager.NavigateTo("/orders/create");
    }

    public async Task SelectOrderAsync(OrderDto order)
    {
        ClearPageError();
        ClearModalError();

        try
        {
            await OrderState.LoadOrderByIdAsync(order.Id);

            if (SelectedOrder is null)
            {
                SetPageError("Ordren kunne ikke hentes.");
                return;
            }

            EditableOrder = MapToEditableOrder(SelectedOrder);
            IsEditModalOpen = true;
            ResetNewDetailInputs();
        }
        catch (Exception ex)
        {
            SetPageError($"Fejl ved hentning af valgt ordre");
        }
    }

    public void CloseEditModal()
    {
        IsEditModalOpen = false;
        EditableOrder = null;
        OrderState.ClearSelectedOrder();
        ResetNewDetailInputs();
        ClearModalError();
    }

    private bool CanAddProductToOrder(ProductDto product, int quantityToAdd, out string errorMessage)
    {
        errorMessage = string.Empty;

        if (EditableOrder is null)
        {
            errorMessage = "Der er ingen ordre klar til redigering.";
            return false;
        }

        if (quantityToAdd <= 0)
        {
            errorMessage = "Antal skal være større end 0.";
            return false;
        }

        if (product.UnitStatus != UnitStatus.InStock)
        {
            errorMessage = "Produktet kan ikke tilføjes, fordi det ikke er på lager.";
            return false;
        }

        int existingQuantityInOrder = EditableOrder.OrderDetails
            .Where(detail => detail.ProductId == product.Id)
            .Sum(detail => detail.Quantity);

        int requestedTotalQuantity = existingQuantityInOrder + quantityToAdd;

        if (requestedTotalQuantity > product.UnitStock)
        {
            errorMessage =
                $"Der er ikke nok på lager. Produktet har {product.UnitStock} stk. på lager, og ordren forsøger at bruge {requestedTotalQuantity} stk.";
            return false;
        }

        return true;
    }

    public void AddOrderDetail()
    {
        ClearModalError();

        if (EditableOrder is null)
        {
            SetModalError("Der er ingen ordre klar til redigering.");
            return;
        }

        if (NewProductId == Guid.Empty)
        {
            SetModalError("Der skal vælges et produkt.");
            return;
        }

        ProductDto? selectedProduct = Products.FirstOrDefault(product => product.Id == NewProductId);

        if (selectedProduct is null)
        {
            SetModalError("Det valgte produkt blev ikke fundet.");
            return;
        }

        if (!CanAddProductToOrder(selectedProduct, NewQuantity, out string errorMessage))
        {
            SetModalError(errorMessage);
            return;
        }

        EditableOrder.OrderDetails.Add(new EditableOrderDetailViewModel
        {
            Id = Guid.NewGuid(),
            OrderId = EditableOrder.Id,
            ProductId = NewProductId,
            Quantity = NewQuantity,
            UnitPrice = selectedProduct.UnitPrice
        });

        RecalculateTotalPrice();
        ResetNewDetailInputs();
    }

    public void RemoveGroupedProduct(Guid productId, decimal unitPrice)
    {
        ClearModalError();

        if (EditableOrder is null)
        {
            SetModalError("Der er ingen ordre klar til redigering.");
            return;
        }

        EditableOrder.OrderDetails.RemoveAll(detail =>
            detail.ProductId == productId &&
            detail.UnitPrice == unitPrice);

        RecalculateTotalPrice();
    }

    public async Task UpdateSelectedOrderAsync()
    {
        ClearModalError();

        if (EditableOrder is null)
        {
            SetModalError("Der skal vælges en ordre før opdatering.");
            return;
        }

        if (HasDuplicateProductsWithDifferentPrice())
        {
            SetModalError(GetDuplicateProductPriceError());
            return;
        }

        try
        {
            List<UpdateOrderDetailRequest> orderDetails = EditableOrder.OrderDetails
                .Select(detail => new UpdateOrderDetailRequest
                {
                    Id = detail.Id,
                    OrderId = EditableOrder.Id,
                    ProductId = detail.ProductId,
                    Quantity = detail.Quantity,
                    UnitPrice = detail.UnitPrice
                })
                .ToList();

            UpdateOrderRequest request = new()
            {
                Id = EditableOrder.Id,
                CustomerId = EditableOrder.CustomerId,
                OrderDate = EditableOrder.OrderDate,
                TotalPrice = orderDetails.Sum(detail => detail.Quantity * detail.UnitPrice),
                OrderDetails = orderDetails
            };

            Console.WriteLine($"Saving order {request.Id}");
            Console.WriteLine($"OrderDetails count: {request.OrderDetails.Count}");

            foreach (UpdateOrderDetailRequest detail in request.OrderDetails)
            {
                Console.WriteLine(
                    $"Detail -> Id: {detail.Id}, OrderId: {detail.OrderId}, ProductId: {detail.ProductId}, Quantity: {detail.Quantity}, UnitPrice: {detail.UnitPrice}");
            }

            await OrderState.UpdateOrderAsync(EditableOrder.Id, request);

            CloseEditModal();
            await LoadPageDataAsync();
        }
        catch (Exception ex)
        {
            SetModalError($"Fejl ved opdatering af ordre");
        }
    }

    public async Task DeleteOrderAsync(Guid id)
    {
        ClearPageError();

        try
        {
            await OrderState.DeleteOrderAsync(id);

            if (SelectedOrder?.Id == id)
            {
                CloseEditModal();
            }
        }
        catch (Exception ex)
        {
            SetPageError($"Fejl ved sletning af ordre");
        }
    }

    private EditableOrderViewModel MapToEditableOrder(OrderDto order)
    {
        return new EditableOrderViewModel
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            OrderDate = order.OrderDate,
            TotalPrice = order.TotalPrice,
            OrderDetails = order.OrderDetails
                .Select(detail => new EditableOrderDetailViewModel
                {
                    Id = detail.Id,
                    OrderId = detail.OrderId,
                    ProductId = detail.ProductId,
                    Quantity = detail.Quantity,
                    UnitPrice = detail.UnitPrice
                })
                .ToList()
        };
    }

    private void UpdateNewUnitPriceFromSelectedProduct()
    {
        if (NewProductId == Guid.Empty)
        {
            NewUnitPrice = 0;
            return;
        }

        ProductDto? selectedProduct = Products.FirstOrDefault(product => product.Id == NewProductId);

        if (selectedProduct is null)
        {
            NewUnitPrice = 0;
            return;
        }

        NewUnitPrice = selectedProduct.UnitPrice;
    }

    private void RecalculateTotalPrice()
    {
        if (EditableOrder is null)
        {
            return;
        }

        EditableOrder.TotalPrice = EditableOrder.OrderDetails
            .Sum(detail => detail.Quantity * detail.UnitPrice);
    }

    private void ResetNewDetailInputs()
    {
        NewProductId = Guid.Empty;
        NewQuantity = 1;
        NewUnitPrice = 0;
    }

    private bool HasDuplicateProductsWithDifferentPrice()
    {
        if (EditableOrder is null)
        {
            return false;
        }

        return OrderDetailGroupingHelper.HasDuplicateProductsWithDifferentPrice(
            EditableOrder.OrderDetails);
    }

    private string GetDuplicateProductPriceError()
    {
        if (EditableOrder is null)
        {
            return string.Empty;
        }

        return OrderDetailGroupingHelper.GetDuplicateProductPriceError(
            EditableOrder.OrderDetails,
            GetProductDisplayName);
    }

    public string GetCustomerDisplayName(Guid customerId)
    {
        CustomerDto? customer = Customers.FirstOrDefault(c => c.Id == customerId);
        return customer?.Name ?? customerId.ToString();
    }

    public string GetProductDisplayName(Guid productId)
    {
        ProductDto? product = Products.FirstOrDefault(product => product.Id == productId);
        return product?.Name ?? productId.ToString();
    }

    private void ClearPageError()
    {
        PageErrorMessage = string.Empty;
    }

    private void SetPageError(string message)
    {
        PageErrorMessage = message;
    }

    private void ClearModalError()
    {
        ModalErrorMessage = string.Empty;
    }

    private void SetModalError(string message)
    {
        ModalErrorMessage = message;
    }
}