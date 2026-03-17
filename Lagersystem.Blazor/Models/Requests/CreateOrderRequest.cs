namespace Lagersystem.Blazor.Models.Requests;

public class CreateOrderRequest
{
    // API'et forventer en komplet Order-lignende model ved POST.
    public Guid Id { get; set; }

    public Guid CustomerId { get; set; }

    public DateTime OrderDate { get; set; }

    public decimal TotalPrice { get; set; }

    // OrderDetails sendes som tom liste i første version,
    // så oprettelse kan fungere uden detail-logik endnu.
    public List<CreateOrderDetailRequest> OrderDetails { get; set; } = new();
}