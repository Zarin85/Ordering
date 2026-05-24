using Ordering.Application.Events;
using Ordering.Domain.Orders.Events;

namespace Ordering.Application.EventHandlers;

public class OrderPlacedEventHandler : IDomainEventHandler<OrderPlacedDomainEvent>
{
    public Task HandleAsync(OrderPlacedDomainEvent domainEvent, CancellationToken ct = default)
    {
        Console.WriteLine($"[Domain Event] Order placed: {domainEvent.OrderId} at {domainEvent.OccurredAt}");
        return Task.CompletedTask;
    }
}
