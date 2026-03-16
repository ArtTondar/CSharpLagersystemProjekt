using Lagersystem.Blazor.Models.Dtos;
using Lagersystem.Blazor.Models.Requests;
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

    public async Task LoadOrdersByCustomerIdAsync(Guid customerId)
    {
        IsLoading = true;

        try
        {
            Orders = await _orderService.GetOrdersByCustomerIdAsync(customerId);
        }
        finally
        {
            IsLoading = false;
        }
    }

    public async Task LoadOrdersByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        IsLoading = true;

        try
        {
            Orders = await _orderService.GetOrdersByDateRangeAsync(startDate, endDate);
        }
        finally
        {
            IsLoading = false;
        }
    }

    public async Task LoadOrdersByTotalPriceAsync(decimal totalPrice)
    {
        IsLoading = true;

        try
        {
            Orders = await _orderService.GetOrdersByTotalPriceAsync(totalPrice);
        }
        finally
        {
            IsLoading = false;
        }
    }

    public async Task CreateOrderAsync(CreateOrderRequest request)
    {
        IsLoading = true;

        try
        {
            // Opretter en ny ordre via service-laget.
            await _orderService.CreateOrderAsync(request);

            // Genindlæser listen bagefter.
            await LoadOrdersAsync();
        }
        finally
        {
            IsLoading = false;
        }
    }

    public async Task UpdateOrderAsync(Guid id, UpdateOrderRequest request)
    {
        IsLoading = true;

        try
        {
            // Placeholder:
            // Service-metoden findes, men backend er endnu ikke sikker at ramme
            // for update, så denne kan kommenteres ud senere hvis nødvendigt.
            await _orderService.UpdateOrderAsync(id, request);

            await LoadOrdersAsync();
        }
        finally
        {
            IsLoading = false;
        }
    }

    public async Task DeleteOrderAsync(Guid id)
    {
        IsLoading = true;

        try
        {
            // Sletter ordren via service-laget.
            await _orderService.DeleteOrderAsync(id);

            // Hvis den slettede ordre var valgt i UI'et,
            // nulstilles SelectedOrder bagefter.
            if (SelectedOrder?.Id == id)
            {
                SelectedOrder = null;
            }

            // Genindlæser listen efter sletning,
            // så tabellen viser de nyeste data.
            await LoadOrdersAsync();
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