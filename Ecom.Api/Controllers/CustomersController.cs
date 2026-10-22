
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ecom.Application.DTOs;
using Ecom.Application.Services;
using Ecom.Domain.Constants;

namespace Ecom.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = Roles.Admin)]
public class CustomersController : ControllerBase
{
    private readonly CustomerService _service;

    public CustomersController(CustomerService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<CustomerDto>>> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpPost]

    public async Task<ActionResult<CustomerDto>> Create(
        [FromBody] CreateCustomerDto dto)
    {
        try
        {
            var customer = await _service.CreateAsync(dto);
            return CreatedAtAction(
                nameof (GetById),
                new {id = customer.Id}, 
                customer);
    
        }catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
            
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerDto>> GetById(string id)
    {
        var product = await _service.GetByIdAsync(id);

        if (product is null)
            return NotFound(new { message = "Không tìm thấy khách hàng." });

        return Ok(product);
    }

    
}
