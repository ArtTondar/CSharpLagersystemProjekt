namespace Lagersystem.Blazor.Models.Requests;

public class CreateOrderRequest
{
    // API'et forventer en komplet Order-lignende model ved POST.
    public Guid Id { get; set; }

    public Guid CustomerId { get; set; }

    public DateTime OrderDate { get; set; }

    public decimal TotalPrice { get; set; }

    // Indeholder alle ordrelinjer, som skal gemmes sammen med ordren.
    public List<CreateOrderDetailRequest> OrderDetails { get; set; } = new();
}