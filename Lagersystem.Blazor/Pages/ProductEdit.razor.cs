using Lagersystem.Blazor.Models.Dtos;
using Lagersystem.Blazor.Models.Requests;
using Lagersystem.Blazor.State;
using Microsoft.AspNetCore.Components;

namespace Lagersystem.Blazor.Pages;

public partial class ProductEdit
{
    [Parameter]
    public Guid Id { get; set; }

    [Inject]
    public ProductState ProductState { get; set; } = default!;

    [Inject]
    public NavigationManager NavigationManager { get; set; } = default!;

    // Request-model der bindes til formularen.
    public UpdateProductRequest? EditRequest { get; set; }

    // Bruges til at styre visning af fejlbesked.
    public string ErrorMessage { get; set; } = string.Empty;

    // Bruges til at deaktivere gem-knappen under save.
    public bool IsSaving { get; set; }

    // Gør markup mere læsbar.
    public bool HasError => !string.IsNullOrWhiteSpace(ErrorMessage);

    // Gør markup mere læsbar.
    public bool IsLoading => ProductState.IsLoading;

    // Bruges til dropdown med mulige statusværdier.
    public IReadOnlyList<UnitStatus> UnitStatuses { get; } =
        Enum.GetValues<UnitStatus>();

    protected override async Task OnInitializedAsync()
    {
        await LoadProductAsync();
    }

    private async Task LoadProductAsync()
    {
        ClearError();

        try
        {
            // Henter det valgte produkt via state-laget.
            await ProductState.LoadProductByIdAsync(Id);

            ProductDto? product = ProductState.SelectedProduct;

            // Stopper hvis produktet ikke blev fundet.
            if (product is null)
            {
                SetError("Produktet blev ikke fundet.");
                return;
            }

            // Mapper DTO til request-model,
            // så formularen kan redigere værdierne.
            EditRequest = new UpdateProductRequest
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                UnitPrice = product.UnitPrice,
                Size = product.Size,
                Warehouse = product.Warehouse,
                UnitStock = product.UnitStock,
                UnitStatus = product.UnitStatus
            };
        }
        catch (Exception ex)
        {
            SetError($"Fejl ved hentning af produkt");
        }
    }

    private async Task SaveAsync()
    {
        // Guard clause:
        // Gemning kan ikke fortsætte uden model.
        if (EditRequest is null)
        {
            SetError("Produktdata kunne ikke indlæses.");
            return;
        }

        ClearError();
        IsSaving = true;

        try
        {
            // Sender de redigerede værdier til state-laget,
            // som derefter kalder service og API.
            await ProductState.UpdateProductAsync(Id, EditRequest);

            // Navigerer tilbage til produktoversigten efter vellykket gemning.
            NavigationManager.NavigateTo("/products");
        }
        catch (Exception ex)
        {
            SetError($"Fejl ved gemning af produkt");
        }
        finally
        {
            IsSaving = false;
        }
    }

    private void Cancel()
    {
        // Går tilbage til produktoversigten uden at gemme ændringer.
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