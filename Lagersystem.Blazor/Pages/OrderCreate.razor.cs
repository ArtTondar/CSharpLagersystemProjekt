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
    public NavigationManager NavigationManager { get; set; } = default!;

    // Request-model der bindes til formularen.
    public CreateOrderRequest? CreateRequest { get; set; }

    // CustomerId bindes som tekst, så input kan valideres sikkert
    // før det konverteres til Guid.
    public string CustomerIdText { get; set; } = string.Empty;

    // Fejltekst som vises hvis oprettelsen fejler.
    public string ErrorMessage { get; set; } = string.Empty;

    // Bruges til at deaktivere knappen under oprettelse.
    public bool IsSaving { get; set; }

    // Gør markup mere læsbar.
    public bool HasError => !string.IsNullOrWhiteSpace(ErrorMessage);

    protected override void OnInitialized()
    {
        // Initialiserer request-modellen med systemfelter,
        // som API'et forventer ved oprettelse.
        CreateRequest = new CreateOrderRequest
        {
            Id = Guid.NewGuid(),
            OrderDate = DateTime.Now,
            TotalPrice = 0,
            OrderDetails = new List<CreateOrderDetailRequest>()
        };
    }

    public async Task CreateAsync()
    {
        // Guard clause:
        // Oprettelse kan ikke fortsætte uden model.
        if (CreateRequest is null)
        {
            SetError("Ordredata kunne ikke initialiseres.");
            return;
        }

        ClearError();

        // Validerer at CustomerId er et gyldigt Guid.
        if (!Guid.TryParse(CustomerIdText, out Guid customerId))
        {
            SetError("Customer Id skal være et gyldigt Guid.");
            return;
        }

        CreateRequest.CustomerId = customerId;
        IsSaving = true;

        try
        {
            // Opretter ordren via state-laget.
            OrderDto createdOrder = await OrderState.CreateOrderAsync(CreateRequest);

            // Navigerer tilbage til ordreoversigten efter vellykket oprettelse.
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
        // Går tilbage til ordreoversigten uden at oprette noget.
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