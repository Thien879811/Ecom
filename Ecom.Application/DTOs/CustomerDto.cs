using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom.Application.DTOs;

public record CustomerDto(
    string Id,
    string CustomerName,
    string? Phone,
    string? Address,
    int Point
    );
public record CreateCustomerDto(
    string CustomerName,
    string? Phone,
    string? Address,
    int Point
    );
public record UpdateCustomerDto(
    string CustomerName,
    string? Phone,
    string? Address,
    int Point
    );
