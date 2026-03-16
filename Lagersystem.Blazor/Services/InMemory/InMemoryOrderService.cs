using Lagersystem.Blazor.Models.Dtos;
using Lagersystem.Blazor.Models.Requests;
using Lagersystem.Blazor.Services.Abstractions;

namespace Lagersystem.Blazor.Services.InMemory;

public class InMemoryOrderService : IOrderService
{
    private static readonly List<OrderDto> _orders =
    [
        new OrderDto
        {
            Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            CustomerId = Guid.Parse("99999999-9999-9999-9999-999999999999"),
            OrderDate = DateTime.Today.AddDays(-2),
            TotalPrice = 998.90m
        }
    ];

    private static readonly List<ProductDto> _products =
    [
        new ProductDto
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Name = "T-Shirt Basic",
            Description = "Sort basic t-shirt",
            UnitPrice = 149.95m,
            Size = 42,
            Warehouse = "Aarhus",
            UnitStock = 25,
            UnitStatus = UnitStatus.InStock
        },
        new ProductDto
        {
            Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            Name = "Sneakers Street",
            Description = "Hvide sneakers",
            UnitPrice = 699.00m,
            Size = 43,
            Warehouse = "Silkeborg",
            UnitStock = 12,
            UnitStatus = UnitStatus.Reserved
        }
    ];

    private static readonly List<InMemoryOrderDetail> _orderDetails =
    [
        new InMemoryOrderDetail
        {
            Id = Guid.NewGuid(),
            OrderId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            ProductId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Quantity = 2,
            UnitPrice = 149.95m
        },
        new InMemoryOrderDetail
        {
            Id = Guid.NewGuid(),
            OrderId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            ProductId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            Quantity = 1,
            UnitPrice = 699.00m
        }
    ];

    public Task<IReadOnlyList<OrderDto>> GetOrdersAsync()
    {
        return Task.FromResult<IReadOnlyList<OrderDto>>(_orders.ToList());
    }

    public Task<OrderDto?> GetOrderByIdAsync(Guid id)
    {
        OrderDto? order = _orders.FirstOrDefault(o => o.Id == id);
        return Task.FromResult(order);
    }

    public Task<OrderDetailsDto?> GetOrderDetailsByIdAsync(Guid id)
    {
        OrderDto? order = _orders.FirstOrDefault(o => o.Id == id);
        if (order is null)
        {
            return Task.FromResult<OrderDetailsDto?>(null);
        }

        List<OrderLineDto> lines = _orderDetails
            .Where(detail => detail.OrderId == id)
            .Join(
                _products,
                detail => detail.ProductId,
                product => product.Id,
                (detail, product) => new OrderLineDto
                {
                    Id = detail.Id,
                    OrderId = detail.OrderId,
                    ProductId = detail.ProductId,
                    ProductName = product.Name,
                    ProductStatus = product.UnitStatus,
                    Quantity = detail.Quantity,
                    UnitPrice = detail.UnitPrice
                })
            .ToList();

        var details = new OrderDetailsDto
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            OrderDate = order.OrderDate,
            TotalPrice = order.TotalPrice,
            OrderLines = lines
        };

        return Task.FromResult<OrderDetailsDto?>(details);
    }

    public Task CreateOrderAsync(CreateOrderRequest request)
    {
        var newOrder = new OrderDto
        {
            Id = Guid.NewGuid(),
            CustomerId = request.CustomerId,
            OrderDate = DateTime.Now,
            TotalPrice = request.TotalPrice
        };

        _orders.Add(newOrder);
        return Task.CompletedTask;
    }

    public Task UpdateOrderAsync(Guid id, UpdateOrderRequest request)
    {
        OrderDto? order = _orders.FirstOrDefault(o => o.Id == id);
        if (order is null)
        {
            return Task.CompletedTask;
        }

        order.CustomerId = request.CustomerId;
        order.TotalPrice = request.TotalPrice;

        return Task.CompletedTask;
    }

    public Task DeleteOrderAsync(Guid id)
    {
        OrderDto? order = _orders.FirstOrDefault(o => o.Id == id);
        if (order is not null)
        {
            _orders.Remove(order);
        }

        _orderDetails.RemoveAll(detail => detail.OrderId == id);

        return Task.CompletedTask;
    }

    private sealed class InMemoryOrderDetail
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}