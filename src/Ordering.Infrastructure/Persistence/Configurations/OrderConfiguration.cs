using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Orders;
using Ordering.Domain.ValueObjects;

namespace Ordering.Infrastructure.Persistence.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);

        builder.Property(o => o.Status)
            .HasConversion<string>();

        // CustomerId value object — stored as a single Guid column
        builder.Property(o => o.CustomerId)
            .HasConversion(
                id => id.Value,
                value => CustomerId.Of(value))
            .HasColumnName("CustomerId");

        // Money value object — stored as two columns
        builder.OwnsOne(o => o.TotalAmount, money =>
        {
            money.Property(m => m.Amount).HasColumnName("TotalAmount");
            money.Property(m => m.Currency).HasColumnName("Currency");
        });

        // Address value object — stored as three columns
        builder.OwnsOne(o => o.ShippingAddress, address =>
        {
            address.Property(a => a.Street).HasColumnName("ShippingStreet");
            address.Property(a => a.City).HasColumnName("ShippingCity");
            address.Property(a => a.Country).HasColumnName("ShippingCountry");
        });

        builder.HasMany(o => o.OrderLines)
            .WithOne()
            .HasForeignKey(l => l.OrderId);

        // Prevent EF from trying to map domain events
        builder.Ignore(o => o.DomainEvents);
    }
}
