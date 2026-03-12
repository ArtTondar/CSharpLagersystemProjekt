using Lagersystem.Blazor.Models.Dtos;

namespace Lagersystem.Blazor.Services.Abstractions;

public interface ICustomerService
{
    Task<IReadOnlyList<CustomerDto>> GetCustomersAsync();
    Task<CustomerDto?> GetCustomerByIdAsync(Guid id);
}