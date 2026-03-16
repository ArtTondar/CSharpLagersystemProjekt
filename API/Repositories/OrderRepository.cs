using API.Models;
using API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _dbContext;

        public OrderRepository(AppDbContext appDbContext)
        {
            _dbContext = appDbContext;
        }

        public async Task<Order> Create(Order order)
        {
            await _dbContext.Orders.AddAsync(order);
            await _dbContext.SaveChangesAsync();

            return order;
        }

        public async Task Delete(Order order)
        {
            _dbContext.Remove(order);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Order>> GetAll()
        {
            return await _dbContext.Orders.ToListAsync();
        }

        public async Task<List<Order>> GetByDate(DateTime start, DateTime? end = null)
        {
            if (end == null)
            {
                return await _dbContext.Orders
                    .Where(o => o.OrderDate >= start)
                    .ToListAsync();
            }

            return await _dbContext.Orders
                .Where(o => o.OrderDate >= start && o.OrderDate <= end)
                .ToListAsync();
        }

        public async Task<Order?> GetById(Guid id)
        {
            return await _dbContext.Orders.FirstOrDefaultAsync(o=> o.Id == id);
        }

        public async Task<List<Order>> GetByTotalPrice(decimal totalPrice)
        {
            return  await _dbContext.Orders.Where(o=> o.TotalPrice == totalPrice).ToListAsync();
        }

        public async Task<List<Order>> GetByCustomerId(Guid customerId)
        {
            return await _dbContext.Orders.Where(o => o.CustomerId == customerId).ToListAsync();
        }

        public async Task Update(Order order)
        {
            _dbContext.Orders.Update(order);
            await _dbContext.SaveChangesAsync();
        }
    }
}
