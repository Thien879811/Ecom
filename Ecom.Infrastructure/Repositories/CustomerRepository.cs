using Ecom.Application.Interfaces;
using Ecom.Domain.Entities;
using Ecom.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace Ecom.Infrastructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    public readonly AppDbContext _context;

    public CustomerRepository (AppDbContext context)
    {
        _context = context;
    }

    public Task<List<Customer>> GetAllAsync()
    {
        return _context.Customers.ToListAsync();
    }

    public Task<Customer?> GetByIdAsync(string id)
    {
        return _context.Customers.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task AddAsync(Customer customer)
    {
        await _context.Customers.AddAsync(customer);
    }

    public Task SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }

    public Task DeleteAsync(Customer customer)
    {
        _context.Customers.Remove(customer);
        return Task.CompletedTask;
    }
}