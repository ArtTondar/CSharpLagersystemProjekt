namespace API.Models
{
    public class Customer
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public string Phone { get; set; }
        public string CVR { get; set; } = "22222222";
        public List<Order> Orders { get; set; } = new List<Order>();

    }
}
