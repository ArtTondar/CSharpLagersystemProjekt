using Lagersystem.Blazor.Models.Dtos;

namespace Lagersystem.Blazor.Models.Requests;

public class CreateProductRequest
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal UnitPrice { get; set; }

    public int Size { get; set; }

    public string Warehouse { get; set; } = string.Empty;

    public int UnitStock { get; set; }

    public UnitStatus UnitStatus { get; set; }
}