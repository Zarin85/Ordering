using Ordering.Application.Orders.DTOs;
using Ordering.Application.Orders.Requests;

namespace Ordering.Application.Orders;

public interface IOrderService
{
    Task<Guid> PlaceOrderAsync(PlaceOrderRequest request, CancellationToken ct = default);
    Task ConfirmOrderAsync(Guid id, CancellationToken ct = default);
    Task CancelOrderAsync(Guid id, CancellationToken ct = default);
    Task<OrderDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<OrderDto>> GetAllAsync(CancellationToken ct = default);
}
