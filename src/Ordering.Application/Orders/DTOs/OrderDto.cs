namespace Ordering.Application.Orders.DTOs;

public record OrderDto(
    Guid Id,
    Guid CustomerId,
    string Street,
    string City,
    string Country,
    string Status,
    decimal TotalAmount,
    string Currency,
    DateTime PlacedAt,
    List<OrderLineDto> OrderLines
);
