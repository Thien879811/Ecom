using Ecom.Application.DTOs;
using Ecom.Application.Interfaces;
using Ecom.Domain.Entities;

namespace Ecom.Application.Services;

public class CustomerService
{
    private readonly ICustomerRepository _repository;

    public CustomerService (ICustomerRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<CustomerDto>> GetAllAsync(){
        var customers = await _repository.GetAllAsync();
        return customers.Select(ToDto).ToList();
    }

    public async Task<CustomerDto> CreateAsync(
        CreateCustomerDto dto)
    {
        Validate(dto.CustomerName, dto.Phone, dto.Address);

        var customer = new Customer { 
            CustomerName = dto.CustomerName,
            Phone = dto.Phone,
            Address = dto.Address,
            Point = dto.Point,
        };
        
        await _repository.AddAsync(customer);
        await _repository.SaveChangesAsync();

        return ToDto(customer);

    }


    public async Task<CustomerDto?>GetByIdAsync(string id)
    {
        var customer = await _repository.GetByIdAsync(id);
        return customer is null ? null : ToDto(customer);
    }

    private static CustomerDto ToDto(Customer c)
    {
        return new CustomerDto(
            c.Id,
            c.CustomerName,
            c.Phone,
            c.Address,
            c.Point
        );
    }

    private static void Validate(
        string CustomerName, string? Phone, string? Address)
    {
        if (string.IsNullOrWhiteSpace(CustomerName))
        {
            throw new ArgumentException("Tên khách hàng không được để trống");
        }
    }
}
