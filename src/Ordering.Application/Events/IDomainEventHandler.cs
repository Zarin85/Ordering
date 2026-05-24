using Ordering.Domain.Common;

namespace Ordering.Application.Events;

public interface IDomainEventHandler<TEvent> where TEvent : IDomainEvent
{
    Task HandleAsync(TEvent domainEvent, CancellationToken ct = default);
}
