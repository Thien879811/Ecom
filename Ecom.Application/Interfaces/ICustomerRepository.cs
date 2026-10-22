using Ecom.Domain.Entities;
namespace Ecom.Application.Interfaces;

public interface ICustomerRepository {
    Task<List<Customer>> GetAllAsync();
    Task<Customer?> GetByIdAsync(string id);
    Task AddAsync(Customer customer);
    Task SaveChangesAsync();
    Task DeleteAsync(Customer customer);
}
