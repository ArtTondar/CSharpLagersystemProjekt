using Lagersystem.Blazor.Models.Dtos;
using Lagersystem.Blazor.Models.Requests;
using Lagersystem.Blazor.Services.Abstractions;

namespace Lagersystem.Blazor.State;

public class OrderState
{
    private readonly IOrderService _orderService;

    public IReadOnlyList<OrderDto> Orders { get; private set; } = new List<OrderDto>();
    public OrderDto? SelectedOrder { get; private set; }
    public OrderDetailsDto? SelectedOrderDetails { get; private set; }
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

    public async Task LoadOrderDetailsByIdAsync(Guid id)
    {
        IsLoading = true;

        try
        {
            SelectedOrderDetails = await _orderService.GetOrderDetailsByIdAsync(id);
        }
        finally
        {
            IsLoading = false;
        }
    }

    public async Task CreateOrderAsync(CreateOrderRequest request)
    {
        await _orderService.CreateOrderAsync(request);
        await LoadOrdersAsync();
    }

    public async Task UpdateOrderAsync(Guid id, UpdateOrderRequest request)
    {
        await _orderService.UpdateOrderAsync(id, request);
        await LoadOrdersAsync();
        await LoadOrderByIdAsync(id);
    }

    public async Task DeleteOrderAsync(Guid id)
    {
        await _orderService.DeleteOrderAsync(id);
        SelectedOrder = null;
        SelectedOrderDetails = null;
        await LoadOrdersAsync();
    }

    public void ClearSelectedOrder()
    {
        SelectedOrder = null;
        SelectedOrderDetails = null;
    }
}