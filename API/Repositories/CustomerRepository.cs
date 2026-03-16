using API.Models;
using API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _dbContext;
        public CustomerRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Customer> Create(Customer customer)
        {
            await _dbContext.Customers.AddAsync(customer);
            await _dbContext.SaveChangesAsync();

            return customer;
        }

        public async Task Delete(Customer customer)
        {
            _dbContext.Remove(customer);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Customer>> GetAll()
        {
            return await _dbContext.Customers.AsNoTracking().ToListAsync();
        }

        public async Task<Customer?> GetByEmail(string email)
        {
            return await _dbContext.Customers.Where(c=>c.Email == email).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<Customer?> GetById(Guid id)
        {
            return await _dbContext.Customers.Where(c => c.Id == id).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<Customer?> GetByPhone(string phone)
        {
            return await _dbContext.Customers.Where(c => c.Phone == phone).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task Update(Customer customer)
        {
            _dbContext.Customers.Attach(customer);
            _dbContext.Entry(customer).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}
