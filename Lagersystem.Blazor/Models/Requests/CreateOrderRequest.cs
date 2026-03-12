namespace Lagersystem.Blazor.Models.Requests;

public class CreateOrderRequest
{
    public Guid CustomerId { get; set; }

    public decimal TotalPrice { get; set; }
}