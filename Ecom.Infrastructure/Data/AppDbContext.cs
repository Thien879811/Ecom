
using Microsoft.EntityFrameworkCore;
using Ecom.Domain.Entities;

namespace Ecom.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(
        DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();

    public DbSet<Customer> Customers => Set<Customer>();

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Products");

            entity.HasKey(p => p.Id);

            entity.Property(p => p.Name)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(p => p.Price)
                .HasPrecision(18, 2);

            entity.Property(p => p.Quantity)
                .IsRequired();

            entity.Property(p => p.CreatedAt)
                .IsRequired();
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("Customer");

            entity.HasKey(p => p.Id);

            entity.Property(p => p.Id)
                .HasMaxLength(36)
                .ValueGeneratedNever();

            entity.Property(p => p.CustomerName)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(p => p.Address);

            entity.Property(p => p.Phone);

            entity.Property(p => p.Point);
        });
    }
}
