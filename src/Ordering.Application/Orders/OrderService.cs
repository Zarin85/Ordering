using FluentValidation;
using Ordering.Application.Events;
using Ordering.Application.Orders.DTOs;
using Ordering.Application.Orders.Exceptions;
using Ordering.Application.Orders.Requests;
using Ordering.Domain.Orders;
using Ordering.Domain.ValueObjects;

namespace Ordering.Application.Orders;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _repository;
    private readonly IDomainEventDispatcher _dispatcher;
    private readonly IValidator<PlaceOrderRequest> _validator;

    public OrderService(
        IOrderRepository repository,
        IDomainEventDispatcher dispatcher,
        IValidator<PlaceOrderRequest> validator)
    {
        _repository = repository;
        _dispatcher = dispatcher;
        _validator = validator;
    }

    public async Task<Guid> PlaceOrderAsync(PlaceOrderRequest request, CancellationToken ct = default)
    {
        await _validator.ValidateAndThrowAsync(request, ct);

        var order = Order.Place(
            CustomerId.Of(request.CustomerId),
            Address.Of(request.Street, request.City, request.Country),
            request.Items.Select(i =>
                (i.ProductId, i.ProductName, Money.Of(i.UnitPrice, "USD"), i.Quantity)).ToList()
        );

        await _repository.AddAsync(order, ct);
        await _dispatcher.DispatchAsync(order.DomainEvents, ct);
        order.ClearDomainEvents();

        return order.Id;
    }

    public async Task ConfirmOrderAsync(Guid id, CancellationToken ct = default)
    {
        var order = await _repository.GetByIdAsync(id, ct)
            ?? throw new NotFoundException($"Order {id} not found.");

        order.Confirm();

        await _repository.UpdateAsync(order, ct);
        await _dispatcher.DispatchAsync(order.DomainEvents, ct);
        order.ClearDomainEvents();
    }

    public async Task CancelOrderAsync(Guid id, CancellationToken ct = default)
    {
        var order = await _repository.GetByIdAsync(id, ct)
            ?? throw new NotFoundException($"Order {id} not found.");

        order.Cancel();

        await _repository.UpdateAsync(order, ct);
        await _dispatcher.DispatchAsync(order.DomainEvents, ct);
        order.ClearDomainEvents();
    }

    public async Task<OrderDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var order = await _repository.GetByIdAsync(id, ct);
        return order is null ? null : MapToDto(order);
    }

    public async Task<IReadOnlyList<OrderDto>> GetAllAsync(CancellationToken ct = default)
    {
        var orders = await _repository.GetAllAsync(ct);
        return orders.Select(MapToDto).ToList();
    }

    private static OrderDto MapToDto(Order order) => new(
        order.Id,
        order.CustomerId.Value,
        order.ShippingAddress.Street,
        order.ShippingAddress.City,
        order.ShippingAddress.Country,
        order.Status.ToString(),
        order.TotalAmount.Amount,
        order.TotalAmount.Currency,
        order.PlacedAt,
        order.OrderLines.Select(l => new OrderLineDto(
            l.ProductId,
            l.ProductName,
            l.UnitPrice.Amount,
            l.Quantity,
            l.LineTotal.Amount
        )).ToList()
    );
}
