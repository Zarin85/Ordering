using Ordering.Application.Events;
using Ordering.Domain.Orders.Events;

namespace Ordering.Application.EventHandlers;

public class OrderCancelledEventHandler : IDomainEventHandler<OrderCancelledDomainEvent>
{
    public Task HandleAsync(OrderCancelledDomainEvent domainEvent, CancellationToken ct = default)
    {
        Console.WriteLine($"[Domain Event] Order cancelled: {domainEvent.OrderId} at {domainEvent.OccurredAt}");
        return Task.CompletedTask;
    }
}
