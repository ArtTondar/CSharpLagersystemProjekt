namespace Lagersystem.Blazor.Models.Dtos;

public class ProductDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal UnitPrice { get; set; }

    public int Size { get; set; }

    public string Warehouse { get; set; } = string.Empty;

    public int UnitStock { get; set; }

    public UnitStatus UnitStatus { get; set; }
}