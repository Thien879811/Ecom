using Ecom.Domain.Entities;

namespace Ecom.Application.Interfaces;

public interface IProductRepository
{
    Task<List<Product>> GetAllAsync();

    Task<Product?> GetByIdAsync(int id);

    Task AddAsync(Product product);

    Task SaveChangesAsync();

    Task DeleteAsync(Product product);
}
