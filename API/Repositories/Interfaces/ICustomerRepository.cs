using API.Models;

namespace API.Repositories.Interfaces
{
    public interface ICustomerRepository
    {
        public Task<Customer?> GetById(Guid id);
        public Task<Customer?> GetByEmail(string email);
        public Task<Customer?> GetByPhone(string phone);
        public Task<List<Customer>> GetAll();
        public Task<Customer> Create(Customer customer);
        public Task Update(Customer customer);
        public Task Delete(Customer customer);
    }
}
