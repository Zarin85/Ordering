using Ordering.Domain.Common;

namespace Ordering.Application.Events;

public interface IDomainEventDispatcher
{
    Task DispatchAsync(IEnumerable<IDomainEvent> events, CancellationToken ct = default);
}
