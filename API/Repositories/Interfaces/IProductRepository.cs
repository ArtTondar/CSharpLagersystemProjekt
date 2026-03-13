using API.Models;

namespace API.Repositories.Interfaces
{
    public interface IProductRepository
    {
        public Task<Product?> GetProductById(Guid Id);

        public Task<List<Product>> GetProducts();

        public Task<List<Product>> GetProductsByName(string name);

        public Task<List<Product>> GetProductsByDescription(string description);

        public Task<List<Product>> GetProductsByUnitPrice(decimal unitPrice);

        public Task<List<Product>> GetProductsBySize(int size);

        public Task<List<Product>> GetProductsByWarehouse(string warehouse);

        public Task<List<Product>> GetProductsByUnitStock(int unitStock);

        public  Task<List<Product>> GetProductsByUnitStatus(UnitStatus unitStatus);

        public Task<Product> CreateProduct(Product product);

        public Task UpdateProduct(Product product);

        public Task DeleteProduct(Product product);

    }
}