namespace Lagersystem.Blazor.Models.Dtos;

public class OrderDto
{
    public Guid Id { get; set; }

    public Guid CustomerId { get; set; }

    public DateTime OrderDate { get; set; }

    public decimal TotalPrice { get; set; }

    public List<OrderDetailDto> OrderDetails { get; set; } = new();
}