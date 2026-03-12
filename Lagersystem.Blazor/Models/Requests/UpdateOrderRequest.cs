namespace Lagersystem.Blazor.Models.Requests;

public class UpdateOrderRequest
{
    public Guid CustomerId { get; set; }

    public decimal TotalPrice { get; set; }
}