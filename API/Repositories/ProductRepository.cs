using API.Models;
using API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _dbContext;

        public ProductRepository(AppDbContext dbContext)
        {
              _dbContext = dbContext;
        }
    
        public async Task<Product?> GetProductById(Guid Id)
        {
            return await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == Id);
        }

        public async Task<List<Product>> GetProducts()
        {
            return await _dbContext.Products.ToListAsync();
        }

        public void UpdateProduct(Product product)
        {
            _dbContext.Products.Update(product);
        }

        public async Task DeleteProduct(Guid productId)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == productId);
            if (product != null)
                _dbContext.Products.Remove(product);
        }

        public async Task<List<Product>> GetProductsByName(string name)
        {
            return await _dbContext.Products.Where(p=> p.Name.Contains(name)).ToListAsync();
        }

        public async Task<List<Product>> GetProductsByDescription(string description)
        {
            return await _dbContext.Products.Where(p => p.Description.Contains(description)).ToListAsync();
        }

        public async Task<List<Product>> GetProductsByUnitPrice(decimal unitPrice)
        {
            return await _dbContext.Products.Where(p=> p.UnitPrice == unitPrice).ToListAsync();             
        }


        public async Task<List<Product>> GetProductsBySize(int size)
        {
            return await _dbContext.Products.Where(p => p.Size == size).ToListAsync();
        }

        public async Task<List<Product>> GetproductsByWarehouse(string warehouse)
        {
            return await _dbContext.Products.Where(p => p.Warehouse == warehouse).ToListAsync();
        }

        public async Task<List<Product>> GetProductsByUnitStatus(UnitStatus unitStatus)
        {
            return await _dbContext.Products.Where(p => p.UnitStatus == unitStatus).ToListAsync();
        }

        public async Task CreateProduct(Product product)
        {
            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Product>> GetProductsByUnitStock(int unitStock)
        {
            return await _dbContext.Products.Where(p => p.UnitStock == unitStock).ToListAsync();
        }

    }
}
