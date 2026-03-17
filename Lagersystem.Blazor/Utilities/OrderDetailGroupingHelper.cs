using Lagersystem.Blazor.Models.ViewModels;

namespace Lagersystem.Blazor.Utilities;

public static class OrderDetailGroupingHelper
{
    public static IReadOnlyList<GroupedOrderDetailViewModel> GroupOrderDetails(
        IEnumerable<EditableOrderDetailViewModel> orderDetails,
        Func<Guid, string> getProductDisplayName)
    {
        ArgumentNullException.ThrowIfNull(orderDetails);
        ArgumentNullException.ThrowIfNull(getProductDisplayName);

        return orderDetails
            .GroupBy(detail => new { detail.ProductId, detail.UnitPrice })
            .Select(group => new GroupedOrderDetailViewModel
            {
                ProductId = group.Key.ProductId,
                ProductName = getProductDisplayName(group.Key.ProductId),
                Quantity = group.Sum(detail => detail.Quantity),
                UnitPrice = group.Key.UnitPrice
            })
            .OrderBy(detail => detail.ProductName)
            .ToList();
    }

    public static bool HasDuplicateProductsWithDifferentPrice(IEnumerable<EditableOrderDetailViewModel> orderDetails)
    {
        ArgumentNullException.ThrowIfNull(orderDetails);

        return orderDetails
            .GroupBy(detail => detail.ProductId)
            .Any(group => group.Select(detail => detail.UnitPrice).Distinct().Count() > 1);
    }

    public static string GetDuplicateProductPriceError(
        IEnumerable<EditableOrderDetailViewModel> orderDetails,
        Func<Guid, string> getProductDisplayName)
    {
        ArgumentNullException.ThrowIfNull(orderDetails);
        ArgumentNullException.ThrowIfNull(getProductDisplayName);

        List<string> invalidProducts = orderDetails
            .GroupBy(detail => detail.ProductId)
            .Where(group => group.Select(detail => detail.UnitPrice).Distinct().Count() > 1)
            .Select(group => getProductDisplayName(group.Key))
            .ToList();

        if (invalidProducts.Count == 0)
        {
            return string.Empty;
        }

        return $"Samme produkt findes flere gange med forskellig pris: {string.Join(", ", invalidProducts)}.";
    }
}