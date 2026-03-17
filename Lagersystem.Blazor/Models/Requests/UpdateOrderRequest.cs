namespace Lagersystem.Blazor.Models.Requests;

public class UpdateOrderRequest
{
    public Guid Id { get; set; }

    public Guid CustomerId { get; set; }

    public DateTime OrderDate { get; set; }

    public decimal TotalPrice { get; set; }

    public List<UpdateOrderDetailRequest> OrderDetails { get; set; } = new();
}

public class UpdateOrderDetailRequest
{
    public Guid Id { get; set; }

    public Guid OrderId { get; set; }

    public Guid ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }
}