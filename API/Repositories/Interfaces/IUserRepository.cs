using API.Models;

namespace API.Repositories.Interfaces
{
    public interface IUserRepository
    {
        public Task<User?> GetById(Guid id);
        public Task<User?> GetByEmail(string email);
        public Task<List<User>> GetAll();
        public Task<User> Create(User user);
        public Task Update(User user);
        public Task Delete(User user);
    }
}
