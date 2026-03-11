namespace API.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }

        public decimal TotalPrice { get; set; }

    }
}
