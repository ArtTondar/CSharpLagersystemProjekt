using Lagersystem.Blazor.Models.Dtos;
using Lagersystem.Blazor.Models.Requests;
using Lagersystem.Blazor.State;
using Microsoft.AspNetCore.Components;

namespace Lagersystem.Blazor.Pages;

public partial class OrderView
{
    [Inject]
    public OrderState OrderState { get; set; } = default!;

    [Inject]
    public NavigationManager NavigationManager { get; set; } = default!;

    public IReadOnlyList<OrderDto> Orders => OrderState.Orders;
    public bool IsLoading => OrderState.IsLoading;

    private OrderFormModel Form { get; set; } = new();
    private bool IsEditMode => Form.Id.HasValue;

    protected override async Task OnInitializedAsync()
    {
        await OrderState.LoadOrdersAsync();
    }

    private void StartCreate()
    {
        Form = new OrderFormModel();
        OrderState.ClearSelectedOrder();
    }

    private void StartEdit(OrderDto order)
    {
        Form = new OrderFormModel
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            TotalPrice = order.TotalPrice
        };
    }

    private void ViewOrder(Guid orderId)
    {
        NavigationManager.NavigateTo($"/order/{orderId}");
    }

    private async Task SaveAsync()
    {
        if (IsEditMode)
        {
            await OrderState.UpdateOrderAsync(Form.Id!.Value, new UpdateOrderRequest
            {
                CustomerId = Form.CustomerId,
                TotalPrice = Form.TotalPrice
            });

            return;
        }

        await OrderState.CreateOrderAsync(new CreateOrderRequest
        {
            CustomerId = Form.CustomerId,
            TotalPrice = Form.TotalPrice
        });

        StartCreate();
    }

    private async Task DeleteAsync(Guid id)
    {
        await OrderState.DeleteOrderAsync(id);
        StartCreate();
    }

    private sealed class OrderFormModel
    {
        public Guid? Id { get; set; }
        public Guid CustomerId { get; set; }
        public decimal TotalPrice { get; set; }
    }
}