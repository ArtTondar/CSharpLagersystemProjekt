using Lagersystem.Blazor.Models.Dtos;
using Lagersystem.Blazor.Services.Abstractions;

namespace Lagersystem.Blazor.State;

public class OrderState
{
    private readonly IOrderService _orderService;

    public IReadOnlyList<OrderDto> Orders { get; private set; } = new List<OrderDto>();

    public OrderDto? SelectedOrder { get; private set; }

    public bool IsLoading { get; private set; }

    public OrderState(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public async Task LoadOrdersAsync()
    {
        IsLoading = true;

        try
        {
            Orders = await _orderService.GetOrdersAsync();
        }
        finally
        {
            IsLoading = false;
        }
    }

    public async Task LoadOrderByIdAsync(Guid id)
    {
        IsLoading = true;

        try
        {
            SelectedOrder = await _orderService.GetOrderByIdAsync(id);
        }
        finally
        {
            IsLoading = false;
        }
    }

    public void ClearSelectedOrder()
    {
        SelectedOrder = null;
    }
}