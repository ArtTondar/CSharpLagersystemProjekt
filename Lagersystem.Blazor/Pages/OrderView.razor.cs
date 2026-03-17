using Lagersystem.Blazor.Models.Dtos;
using Lagersystem.Blazor.State;
using Microsoft.AspNetCore.Components;
using Lagersystem.Blazor.Models.Requests;

namespace Lagersystem.Blazor.Pages;

public partial class OrderView
{
    // OrderState bruges som mellemled mellem UI og service-lag.
    // Komponenten skal ikke selv kende til HttpClient eller API-endpoints.
    [Inject]
    public OrderState OrderState { get; set; } = default!;

    // NavigationManager bruges til at navigere til andre sider,
    // fx siden for oprettelse af en ny ordre.
    [Inject]
    public NavigationManager NavigationManager { get; set; } = default!;

    // Listen der vises i tabellen.
    public IReadOnlyList<OrderDto> Orders => OrderState.Orders;

    // Den valgte ordre bruges til at vise detaljer under tabellen.
    public OrderDto? SelectedOrder => OrderState.SelectedOrder;

    // Bruges til loading-besked og til at deaktivere knappen under hentning.
    public bool IsLoading => OrderState.IsLoading;

    // Fejltekst hvis API-kald fejler.
    public string ErrorMessage { get; set; } = string.Empty;

    // Bruges til at styre om fejlbeskeden skal vises.
    public bool HasError => !string.IsNullOrWhiteSpace(ErrorMessage);

    // Viser tom-besked når listen er tom og der ikke længere hentes data.
    public bool ShowEmptyMessage => !IsLoading && Orders.Count == 0;

    // Når siden åbner første gang, hentes alle ordrer via GET /api/Order.
    protected override async Task OnInitializedAsync()
    {
        await LoadOrdersAsync();
    }

    // Kaldes fra knappen "Genindlæs ordrer".
    public async Task ReloadOrdersAsync()
    {
        await LoadOrdersAsync();
    }

    // Samler logikken til at hente alle ordrer ét sted.
    private async Task LoadOrdersAsync()
    {
        ClearError();

        try
        {
            // Kalder state-laget, som derefter kalder service-laget
            // og til sidst GET /api/Order.
            await OrderState.LoadOrdersAsync();
        }
        catch (Exception ex)
        {
            SetError($"Fejl ved hentning af ordrer: {ex.Message}");
        }
    }

    public void OpenCreateOrderPage()
    {
        // Navigerer til siden for oprettelse af en ny ordre.
        NavigationManager.NavigateTo("/orders/create");
    }

    // Henter den konkrete ordre igen ud fra dens id.
    // Det er nyttigt hvis man vil vise detaljer eller senere lave edit-view.
    public async Task SelectOrderAsync(OrderDto order)
    {
        ClearError();

        try
        {
            await OrderState.LoadOrderByIdAsync(order.Id);
        }
        catch (Exception ex)
        {
            SetError($"Fejl ved hentning af valgt ordre: {ex.Message}");
        }
    }

    public async Task UpdateSelectedOrderAsync()
    {
        ClearError();

        if (SelectedOrder is null)
        {
            SetError("Der skal vælges en ordre før opdatering.");
            return;
        }

        try
        {
            // Bygger en update-request ud fra den valgte ordre.
            // Her genbruges de nuværende værdier som simpel test.
            UpdateOrderRequest request = new()
            {
                Id = SelectedOrder.Id,
                CustomerId = SelectedOrder.CustomerId,
                OrderDate = SelectedOrder.OrderDate,
                TotalPrice = SelectedOrder.TotalPrice
            };

            // Kalder state-laget, som derefter opdaterer via API'et.
            await OrderState.UpdateOrderAsync(SelectedOrder.Id, request);
        }
        catch (Exception ex)
        {
            SetError($"Fejl ved opdatering af ordre: {ex.Message}");
        }
    }

    public async Task DeleteOrderAsync(Guid id)
    {
        ClearError();

        try
        {
            // Kalder state-laget, som derefter sletter ordren via API'et.
            await OrderState.DeleteOrderAsync(id);
        }
        catch (Exception ex)
        {
            SetError($"Fejl ved sletning af ordre: {ex.Message}");
        }
    }

    // Nulstiller tidligere fejl før et nyt API-kald.
    private void ClearError()
    {
        ErrorMessage = string.Empty;
    }

    // Sætter fejlbesked hvis noget går galt.
    private void SetError(string message)
    {
        ErrorMessage = message;
    }
}