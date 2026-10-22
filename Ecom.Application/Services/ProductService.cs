
using Ecom.Application.DTOs;
using Ecom.Application.Interfaces;
using Ecom.Domain.Entities;

namespace Ecom.Application.Services;

public class ProductService
{
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<ProductDto>> GetAllAsync()
    {
        var products = await _repository.GetAllAsync();

        return products.Select(ToDto).ToList();
    }

    public async Task<ProductDto?> GetByIdAsync(int id)
    {
        var product = await _repository.GetByIdAsync(id);

        return product is null ? null : ToDto(product);
    }

    public async Task<ProductDto> CreateAsync(
        CreateProductDto dto)
    {
        Validate(dto.Name, dto.Price, dto.Quantity);

        var product = new Product
        {
            Name = dto.Name.Trim(),
            Price = dto.Price,
            Quantity = dto.Quantity
        };

        await _repository.AddAsync(product);
        await _repository.SaveChangesAsync();

        return ToDto(product);
    }

    public async Task<ProductDto?> UpdateAsync(
        int id, UpdateProductDto dto)
    {
        Validate(dto.Name, dto.Price, dto.Quantity);

        var product = await _repository.GetByIdAsync(id);

        if (product is null)
            return null;

        product.Name = dto.Name.Trim();
        product.Price = dto.Price;
        product.Quantity = dto.Quantity;

        await _repository.SaveChangesAsync();

        return ToDto(product);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = await _repository.GetByIdAsync(id);

        if (product is null)
            return false;

        await _repository.DeleteAsync(product);
        await _repository.SaveChangesAsync();

        return true;
    }

    private static ProductDto ToDto(Product p)
    {
        return new ProductDto(
            p.Id,
            p.Name,
            p.Price,
            p.Quantity,
            p.CreatedAt
        );
    }

    private static void Validate(
        string name, decimal price, int quantity)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Tên sản phẩm không được để trống.");

        if (name.Trim().Length > 200)
            throw new ArgumentException("Tên sản phẩm tối đa 200 ký tự.");

        if (price < 0)
            throw new ArgumentException("Giá không được âm.");

        if (quantity < 0)
            throw new ArgumentException("Số lượng không được âm.");
    }
}
