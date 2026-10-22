namespace Ecom.Application.DTOs;

public record ProductDto(
    int Id,
    string Name,
    decimal Price,
    int Quantity,
    DateTime CreatedAt);

public record CreateProductDto(
    string Name,
    decimal Price,
    int Quantity);

public record UpdateProductDto(
    string Name,
    decimal Price,
    int Quantity);
