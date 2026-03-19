using Lagersystem.Blazor.Models.Dtos;
using Lagersystem.Blazor.State;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Lagersystem.Blazor.Pages;



public partial class ProductView
{

    // IJSRuntime bruges til at kalde JavaScript fra Blazor.
    // Her anvendes det til at vise en browser confirm-dialog før sletning,
    // så brugeren kan bekræfte handlingen.
    [Inject]
    public IJSRuntime JS { get; set; } = default!;


    // ProductState bruges som mellemled mellem UI og service-lag.
    // Komponenten skal kun bede state om data, ikke kalde API direkte.
    [Inject]
    public ProductState ProductState { get; set; } = default!;

    // NavigationManager bruges til at skifte til redigeringssiden
    // for det valgte produkt.
    [Inject]
    public NavigationManager NavigationManager { get; set; } = default!;

    // Listen der vises i tabellen.
    public IReadOnlyList<ProductDto> Products => ProductState.Products;

    // Bruges til at vise loading-besked og deaktivere knapper under hentning.
    public bool IsLoading => ProductState.IsLoading;

    // Fejltekst hvis API-kald fejler.
    public string ErrorMessage { get; set; } = string.Empty;

    // Bruges til at styre visning af fejlbeskeden.
    public bool HasError => !string.IsNullOrWhiteSpace(ErrorMessage);

    // Viser tom-besked når der ikke hentes data, og listen er tom.
    public bool ShowEmptyMessage => !IsLoading && Products.Count == 0;

    // Når siden åbner første gang, hentes alle produkter via GET /api/Product.
    protected override async Task OnInitializedAsync()
    {
        await LoadProductsAsync();
    }

    // Kaldes fra knappen "Genindlæs produkter".
    public async Task ReloadProductsAsync()
    {
        await LoadProductsAsync();
    }

    // Samler logikken til at hente alle produkter ét sted.
    private async Task LoadProductsAsync()
    {
        ClearError();

        try
        {
            // Denne metode skal kalde GET /api/Product i service-laget.
            await ProductState.LoadProductsAsync();
        }
        catch (Exception ex)
        {
            SetError($"Fejl ved hentning af produkter");
        }
    }

    // Navigerer til en separat side for det valgte produkt,
    // hvor redigering og gemning håndteres.
    public void OpenProduct(Guid productId)
    {
        NavigationManager.NavigateTo($"/products/{productId}");
    }


    // Gemmer valgt produkt i state.
    // Kan stadig beholdes midlertidigt, hvis anden eksisterende kode bruger den.
    public void SelectProduct(ProductDto product)
    {
        ProductState.SelectProduct(product);
    }

    // Navigerer til siden for oprettelse af et nyt produkt.
    public void OpenCreateProductPage()
    {
        // Navigerer til siden for oprettelse af et nyt produkt.
        NavigationManager.NavigateTo("/products/create");
    }

    // Sletter et produkt via state-laget
    // og opdaterer tabellen uden genindlæsning.
    public async Task DeleteProductAsync(Guid productId)
    {
        ClearError();

        try
        {
            // Viser en browser confirm dialog.
            bool confirmed = await JS.InvokeAsync<bool>(
                "confirm",
                "Er du sikker på, at du vil slette dette produkt?");

            // Stopper hvis brugeren annullerer.
            if (!confirmed)
            {
                return;
            }

            // Kalder state-laget for at slette produktet.
            await ProductState.DeleteProductAsync(productId);
        }
        catch (Exception ex)
        {
            SetError($"Fejl ved sletning af produkt");
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