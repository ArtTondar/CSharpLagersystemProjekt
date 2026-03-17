using Lagersystem.Blazor.Models.Dtos;
using Lagersystem.Blazor.Models.Requests;
using Lagersystem.Blazor.State;
using Microsoft.AspNetCore.Components;

namespace Lagersystem.Blazor.Pages;

public partial class OrderView
{
    // OrderState bruges som mellemled mellem UI og service-lag.
    // Komponenten skal ikke selv kende til HttpClient eller API-endpoints.
    [Inject]
    public OrderState OrderState { get; set; } = default!;

    // CustomerState bruges til at hente kunder,
    // så ordrelisten kan vise kundenavne i stedet for rå id'er.
    [Inject]
    public CustomerState CustomerState { get; set; } = default!;

    // NavigationManager bruges til at navigere til andre sider,
    // fx siden for oprettelse af en ny ordre.
    [Inject]
    public NavigationManager NavigationManager { get; set; } = default!;

    // Listen der vises i tabellen.
    public IReadOnlyList<OrderDto> Orders => OrderState.Orders;

    // Bruges til opslag af kundenavne ud fra CustomerId.
    public IReadOnlyList<CustomerDto> Customers => CustomerState.Customers;

    // Den valgte ordre bruges til at vise detaljer over tabellen.
    public OrderDto? SelectedOrder => OrderState.SelectedOrder;

    // Bruges til loading-besked og til at deaktivere knapper under hentning.
    public bool IsLoading => OrderState.IsLoading || CustomerState.IsLoading;

    // Fejltekst hvis API-kald fejler.
    public string ErrorMessage { get; set; } = string.Empty;

    // Bruges til at styre om fejlbeskeden skal vises.
    public bool HasError => !string.IsNullOrWhiteSpace(ErrorMessage);

    // Viser tom-besked når listen er tom og der ikke længere hentes data.
    public bool ShowEmptyMessage => !IsLoading && Orders.Count == 0;

    // Når siden åbner første gang, hentes både ordrer og kunder.
    protected override async Task OnInitializedAsync()
    {
        await LoadPageDataAsync();
    }

    // Kaldes fra knappen "Genindlæs ordrer".
    public async Task ReloadOrdersAsync()
    {
        await LoadPageDataAsync();
    }

    // Henter både ordrer og kunder,
    // så tabellen kan vise læsbare kundenavne.
    private async Task LoadPageDataAsync()
    {
        ClearError();

        try
        {
            await OrderState.LoadOrdersAsync();
            await CustomerState.LoadCustomersAsync();
        }
        catch (Exception ex)
        {
            SetError($"Fejl ved hentning af data: {ex.Message}");
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

    // Finder kundenavn ud fra customer-id.
    // Returnerer id som tekst hvis kunden ikke findes i den hentede liste.
    public string GetCustomerDisplayName(Guid customerId)
    {
        CustomerDto? customer = Customers.FirstOrDefault(c => c.Id == customerId);

        return customer?.Name ?? customerId.ToString();
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