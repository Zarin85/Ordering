using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Orders;

namespace Ordering.Infrastructure.Persistence.Configurations;

public class OrderLineConfiguration : IEntityTypeConfiguration<OrderLine>
{
    public void Configure(EntityTypeBuilder<OrderLine> builder)
    {
        builder.HasKey(l => l.Id);

        // Money value objects on OrderLine
        builder.OwnsOne(l => l.UnitPrice, money =>
        {
            money.Property(m => m.Amount).HasColumnName("UnitPrice");
            money.Property(m => m.Currency).HasColumnName("UnitCurrency");
        });

        builder.OwnsOne(l => l.LineTotal, money =>
        {
            money.Property(m => m.Amount).HasColumnName("LineTotal");
            money.Property(m => m.Currency).HasColumnName("LineCurrency");
        });
    }
}
