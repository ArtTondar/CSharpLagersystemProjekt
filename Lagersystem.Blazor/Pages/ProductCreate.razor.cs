using Lagersystem.Blazor.Models.Dtos;
using Lagersystem.Blazor.Models.Requests;
using Lagersystem.Blazor.State;
using Microsoft.AspNetCore.Components;

namespace Lagersystem.Blazor.Pages;

public partial class ProductCreate
{
    [Inject]
    public ProductState ProductState { get; set; } = default!;

    [Inject]
    public NavigationManager NavigationManager { get; set; } = default!;

    // Request-model der bindes til formularen.
    public CreateProductRequest? CreateRequest { get; set; }

    // Fejltekst som vises hvis oprettelsen fejler.
    public string ErrorMessage { get; set; } = string.Empty;

    // Bruges til at deaktivere knappen under oprettelse.
    public bool IsSaving { get; set; }

    // Gør markup mere læsbar.
    public bool HasError => !string.IsNullOrWhiteSpace(ErrorMessage);

    // Bruges til dropdown med mulige statusværdier.
    public IReadOnlyList<UnitStatus> UnitStatuses { get; } =
        Enum.GetValues<UnitStatus>();

    protected override void OnInitialized()
    {
        // Initialiserer formularen med tomme standardværdier.
        CreateRequest = new CreateProductRequest
        {
            Name = string.Empty,
            Description = string.Empty,
            Warehouse = string.Empty
        };
    }

    public async Task CreateAsync()
    {
        // Guard clause:
        // Oprettelse kan ikke fortsætte uden model.
        if (CreateRequest is null)
        {
            SetError("Produktdata kunne ikke initialiseres.");
            return;
        }

        ClearError();
        IsSaving = true;

        try
        {
            // Opretter produktet via state-laget.
            ProductDto createdProduct = await ProductState.CreateProductAsync(CreateRequest);

            // Navigerer tilbage til produktoversigten efter vellykket oprettelse.
            NavigationManager.NavigateTo("/products");
        }
        catch (Exception ex)
        {
            SetError($"Fejl ved oprettelse af produkt: {ex.Message}");
        }
        finally
        {
            IsSaving = false;
        }
    }

    public void Cancel()
    {
        // Går tilbage til produktoversigten uden at oprette noget.
        NavigationManager.NavigateTo("/products");
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