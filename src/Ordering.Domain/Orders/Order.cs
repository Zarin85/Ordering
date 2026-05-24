using Ordering.Domain.Common;
using Ordering.Domain.Orders.Events;
using Ordering.Domain.ValueObjects;

namespace Ordering.Domain.Orders;

public sealed class Order : AggregateRoot<Guid>
{
    private readonly List<OrderLine> _orderLines = new();

    public CustomerId CustomerId { get; private set; } = default!;
    public Address ShippingAddress { get; private set; } = default!;
    public OrderStatus Status { get; private set; }
    public Money TotalAmount { get; private set; } = default!;
    public DateTime PlacedAt { get; private set; }

    public IReadOnlyList<OrderLine> OrderLines => _orderLines.AsReadOnly();

    private Order() { } // EF Core

    public static Order Place(
        CustomerId customerId,
        Address shippingAddress,
        List<(Guid ProductId, string ProductName, Money UnitPrice, int Quantity)> items)
    {
        if (items == null || items.Count == 0)
            throw new DomainException("Order must have at least one item.");

        var order = new Order
        {
            Id = Guid.NewGuid(),
            CustomerId = customerId,
            ShippingAddress = shippingAddress,
            Status = OrderStatus.Pending,
            TotalAmount = Money.Of(0, "USD"),
            PlacedAt = DateTime.UtcNow
        };

        foreach (var item in items)
            order.AddLine(item.ProductId, item.ProductName, item.UnitPrice, item.Quantity);

        order.RaiseDomainEvent(new OrderPlacedDomainEvent(order.Id));
        return order;
    }

    public void Confirm()
    {
        if (Status != OrderStatus.Pending)
            throw new DomainException("Only pending orders can be confirmed.");

        Status = OrderStatus.Confirmed;
        RaiseDomainEvent(new OrderConfirmedDomainEvent(Id));
    }

    public void Cancel()
    {
        if (Status == OrderStatus.Delivered)
            throw new DomainException("Delivered orders cannot be cancelled.");
        if (Status == OrderStatus.Cancelled)
            throw new DomainException("Order is already cancelled.");

        Status = OrderStatus.Cancelled;
        RaiseDomainEvent(new OrderCancelledDomainEvent(Id));
    }

    private void AddLine(Guid productId, string productName, Money unitPrice, int quantity)
    {
        var line = OrderLine.Create(Id, productId, productName, unitPrice, quantity);
        _orderLines.Add(line);
        RecalculateTotal();
    }

    private void RecalculateTotal() =>
        TotalAmount = _orderLines.Aggregate(Money.Of(0, "USD"), (sum, line) => sum.Add(line.LineTotal));
}
