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

            foreach (var order in Orders)
            {
                decimal totalPrice = 0;
                foreach (var orderDetail in order.OrderDetails)
                {
                    totalPrice += orderDetail.UnitPrice * orderDetail.Quantity;
                }
                order.TotalPrice = totalPrice;
            }
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
            if(SelectedOrder!= null)
            {
                decimal totalPrice = 0;
                foreach (var orderDetail in SelectedOrder.OrderDetails)
                {
                    totalPrice += orderDetail.UnitPrice * orderDetail.Quantity;
                }
                SelectedOrder.TotalPrice = totalPrice;
            }
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

    public async Task<OrderDto> CreateOrderAsync(CreateOrderRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        IsLoading = true;

        try
        {
            // Kalder service-laget, som sender POST-request til API'et.
            OrderDto createdOrder = await _orderService.CreateOrderAsync(request);

            // Lægger den nye ordre ind i den lokale state,
            // så listen opdateres uden fuld genindlæsning.
            List<OrderDto> updatedOrders = Orders.ToList();
            updatedOrders.Add(createdOrder);

            Orders = updatedOrders;
            SelectedOrder = createdOrder;

            return createdOrder;
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
            // Opdaterer ordren via service-laget.
            await _orderService.UpdateOrderAsync(id, request);

            // Hvis den opdaterede ordre er valgt i UI'et,
            // hentes den igen for at vise de nyeste data.
            if (SelectedOrder?.Id == id)
            {
                await LoadOrderByIdAsync(id);
            }

            // Genindlæser listen efter opdatering,
            // så tabellen viser de nyeste data.
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