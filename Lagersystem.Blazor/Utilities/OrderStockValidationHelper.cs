using Lagersystem.Blazor.Models.Dtos;
using Lagersystem.Blazor.Models.ViewModels;

namespace Lagersystem.Blazor.Utilities;

public static class OrderStockValidationHelper
{
    public static bool CanAddProduct(
        ProductDto product,
        IEnumerable<EditableOrderDetailViewModel> existingOrderDetails,
        int quantityToAdd,
        out string errorMessage)
    {
        errorMessage = string.Empty;

        if (quantityToAdd <= 0)
        {
            errorMessage = "Antal skal være større end 0.";
            return false;
        }

        // Tilpas hvis UnitStatus er string i stedet for int.
        if (product.UnitStatus != UnitStatus.InStock)
        {
            errorMessage = "Produktet kan ikke tilføjes, fordi det ikke er på lager.";
            return false;
        }

        int existingQuantityInOrder = existingOrderDetails
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
}