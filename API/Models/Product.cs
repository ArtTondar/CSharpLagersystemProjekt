namespace API.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal UnitPrice { get; set; }
        public int Size { get; set; }
        public string Warehouse { get; set; } = "default";
        public int UnitStock { get; set; }
        public UnitStatus UnitStatus { get; set; }
    }
}
