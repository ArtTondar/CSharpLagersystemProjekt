namespace Lagersystem.Blazor.Models.ViewModels;

public class EditableOrderViewModel
{
    public Guid Id { get; set; }

    public Guid CustomerId { get; set; }

    public DateTime OrderDate { get; set; }

    public decimal TotalPrice { get; set; }

    public List<EditableOrderDetailViewModel> OrderDetails { get; set; } = new();
}