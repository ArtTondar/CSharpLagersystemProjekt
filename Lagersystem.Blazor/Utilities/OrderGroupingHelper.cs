using Lagersystem.Blazor.Models.Dtos;
using Lagersystem.Blazor.Models.ViewModels;

namespace Lagersystem.Blazor.Utilities;

public static class OrderGroupingHelper
{
    public static List<GroupedOrderDetailViewModel> GroupDetails(
        IEnumerable<OrderDetailDto> details,
        Func<Guid, string> getProductName)
    {
        return details
            .GroupBy(d => new { d.ProductId, d.UnitPrice })
            .Select(group => new GroupedOrderDetailViewModel
            {
                ProductId = group.Key.ProductId,
                ProductName = getProductName(group.Key.ProductId),
                Quantity = group.Sum(d => d.Quantity),
                UnitPrice = group.Key.UnitPrice
            })
            .OrderBy(d => d.ProductName)
            .ToList();
    }
}