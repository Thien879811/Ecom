
namespace Ecom.Domain.Entities;

public class Customer 
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string CustomerName { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public int Point { get; set; }
}
