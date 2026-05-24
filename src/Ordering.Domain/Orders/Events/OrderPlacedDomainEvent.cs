using Ordering.Domain.Common;

namespace Ordering.Domain.Orders.Events;

public sealed record OrderPlacedDomainEvent(Guid OrderId) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredAt { get; } = DateTime.UtcNow;
}
