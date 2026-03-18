using Lagersystem.Blazor.Models.Dtos;
using Lagersystem.Blazor.Models.Requests;
using Lagersystem.Blazor.State;
using Microsoft.AspNetCore.Components;

namespace Lagersystem.Blazor.Pages;

public partial class OrderCreate
{
    [Inject]
    public OrderState OrderState { get; set; } = default!;

    [Inject]
    public ProductState ProductState { get; set; } = default!;

    [Inject]
    public CustomerState CustomerState { get; set; } = default!;

    [Inject]
    public NavigationManager NavigationManager { get; set; } = default!;

    // Request-model der bindes til formularen.
    public CreateOrderRequest? CreateRequest { get; set; }

    // Fejltekst som vises hvis oprettelsen fejler.
    public string ErrorMessage { get; set; } = string.Empty;

    // Bruges til at deaktivere knappen under oprettelse.
    public bool IsSaving { get; set; }

    // Gør markup mere læsbar.
    public bool HasError => !string.IsNullOrWhiteSpace(ErrorMessage);

    // Produkter bruges i dropdown for ordrelinjer.
    public IReadOnlyList<ProductDto> Products => ProductState.Products;

    // Kunder bruges i dropdown for valg af kunde.
    public IReadOnlyList<CustomerDto> Customers => CustomerState.Customers;

    protected override async Task OnInitializedAsync()
    {
        CreateRequest = new CreateOrderRequest
        {
            Id = Guid.NewGuid(),
            OrderDate = DateTime.Now,
            TotalPrice = 0,
            OrderDetails = new List<CreateOrderDetailRequest>()
        };

        try
        {
            // Henter produkter til produkt-dropdown.
            await ProductState.LoadProductsAsync();

            // Henter kunder til kunde-dropdown.
            await CustomerState.LoadCustomersAsync();
        }
        catch (Exception ex)
        {
            SetError($"Fejl ved hentning af data: {ex.Message}");
            return;
        }

        AddOrderDetail();
    }

    public void AddOrderDetail()
    {
        if (CreateRequest is null)
        {
            return;
        }

        CreateRequest.OrderDetails.Add(new CreateOrderDetailRequest
        {
            Id = Guid.NewGuid(),
            Quantity = 1,
            UnitPrice = 0
        });

        RecalculateTotalPrice();
    }

    public void RemoveOrderDetail(CreateOrderDetailRequest detail)
    {
        if (CreateRequest is null)
        {
            return;
        }

        CreateRequest.OrderDetails.Remove(detail);
        RecalculateTotalPrice();
    }

    public void OnProductChanged(CreateOrderDetailRequest detail)
    {
        ProductDto? selectedProduct = Products.FirstOrDefault(product => product.Id == detail.ProductId);

        if (selectedProduct is not null)
        {
            detail.UnitPrice = selectedProduct.UnitPrice;
        }
        else
        {
            detail.UnitPrice = 0;
        }

        RecalculateTotalPrice();
    }

    public void OnQuantityChanged()
    {
        RecalculateTotalPrice();
    }

    public void RecalculateTotalPrice()
    {
        if (CreateRequest is null)
        {
            return;
        }

        CreateRequest.TotalPrice = CreateRequest.OrderDetails.Sum(detail =>
            detail.Quantity * detail.UnitPrice);
    }

    public async Task CreateAsync()
    {
        if (CreateRequest is null)
        {
            SetError("Ordredata kunne ikke initialiseres.");
            return;
        }

        ClearError();

        if (CreateRequest.CustomerId == Guid.Empty)
        {
            SetError("Der skal vælges en kunde.");
            return;
        }

        if (CreateRequest.OrderDetails.Count == 0)
        {
            SetError("Ordren skal have mindst én produktlinje.");
            return;
        }

        foreach (CreateOrderDetailRequest detail in CreateRequest.OrderDetails)
        {
            if (detail.ProductId == Guid.Empty)
            {
                SetError("Alle ordrelinjer skal have et valgt produkt.");
                return;
            }

            if (detail.Quantity <= 0)
            {
                SetError("Antal skal være større end 0.");
                return;
            }

            if (detail.UnitPrice < 0)
            {
                SetError("Enhedspris kan ikke være negativ.");
                return;
            }

            detail.OrderId = CreateRequest.Id;
        }

        RecalculateTotalPrice();
        IsSaving = true;

        try
        {
            await OrderState.CreateOrderAsync(CreateRequest);
            NavigationManager.NavigateTo("/orders");
        }
        catch (Exception ex)
        {
            SetError($"Fejl ved oprettelse af ordre: {ex.Message}");
        }
        finally
        {
            IsSaving = false;
        }
    }

    public void Cancel()
    {
        NavigationManager.NavigateTo("/orders");
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