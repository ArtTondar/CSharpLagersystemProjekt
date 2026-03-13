using API.Models;

namespace API.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        public Task<Order?> GetById(Guid id);

        public Task<List<Order>> GetAll();

        public Task<List<Order>> GetByDate(DateTime start, DateTime? end=null);

        public Task<List<Order>> GetByTotalPrice(decimal totalPrice);

        public Task<Order> Create(Order order);

        public Task Update(Order order);

        public Task Delete(Order order);
    }
}
