namespace Lagersystem.Blazor.Models.Requests;

public class UpdateOrderRequest
{
    // Id skal være med i body for at matche API'et.
    public Guid Id { get; set; }

    public Guid CustomerId { get; set; }

    public DateTime OrderDate { get; set; }

    public decimal TotalPrice { get; set; }
}