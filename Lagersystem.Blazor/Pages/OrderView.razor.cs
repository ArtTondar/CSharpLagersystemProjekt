using Lagersystem.Blazor.Models.Dtos;
using Lagersystem.Blazor.State;
using Microsoft.AspNetCore.Components;

namespace Lagersystem.Blazor.Pages;

public partial class OrderView
{
    // OrderState bruges som mellemled mellem UI og service-lag.
    // Komponenten skal ikke selv kende til HttpClient eller API-endpoints.
    [Inject]
    public OrderState OrderState { get; set; } = default!;

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