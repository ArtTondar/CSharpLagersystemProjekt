namespace Lagersystem.Blazor.Models.Dtos;

public class OrderLineDto
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }

    public string ProductName { get; set; } = string.Empty;
    public UnitStatus ProductStatus { get; set; }

    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }

    public decimal LineTotal => Quantity * UnitPrice;
}