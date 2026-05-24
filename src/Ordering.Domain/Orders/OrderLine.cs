using Ordering.Domain.Common;
using Ordering.Domain.ValueObjects;

namespace Ordering.Domain.Orders;

public sealed class OrderLine : Entity<Guid>
{
    public Guid OrderId { get; private set; }
    public Guid ProductId { get; private set; }
    public string ProductName { get; private set; } = string.Empty;
    public Money UnitPrice { get; private set; } = default!;
    public int Quantity { get; private set; }
    public Money LineTotal { get; private set; } = default!;

    private OrderLine() { } // EF Core

    internal static OrderLine Create(Guid orderId, Guid productId, string productName, Money unitPrice, int quantity)
    {
        if (quantity <= 0)
            throw new DomainException("Quantity must be greater than zero.");
        if (string.IsNullOrWhiteSpace(productName))
            throw new DomainException("Product name is required.");

        return new OrderLine
        {
            Id = Guid.NewGuid(),
            OrderId = orderId,
            ProductId = productId,
            ProductName = productName,
            UnitPrice = unitPrice,
            Quantity = quantity,
            LineTotal = unitPrice.Multiply(quantity)
        };
    }
}
