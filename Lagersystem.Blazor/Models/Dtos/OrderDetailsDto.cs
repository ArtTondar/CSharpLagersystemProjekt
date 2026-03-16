namespace Lagersystem.Blazor.Models.Dtos;

public class OrderDetailsDto
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalPrice { get; set; }

    public IReadOnlyList<OrderLineDto> OrderLines { get; set; } = new List<OrderLineDto>();
}