using Ordering.Application.Events;
using Ordering.Domain.Orders.Events;

namespace Ordering.Application.EventHandlers;

public class OrderConfirmedEventHandler : IDomainEventHandler<OrderConfirmedDomainEvent>
{
    public Task HandleAsync(OrderConfirmedDomainEvent domainEvent, CancellationToken ct = default)
    {
        Console.WriteLine($"[Domain Event] Order confirmed: {domainEvent.OrderId} at {domainEvent.OccurredAt}");
        return Task.CompletedTask;
    }
}
