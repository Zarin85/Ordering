namespace Ordering.Application.Orders.DTOs;

public record OrderLineDto(
    Guid ProductId,
    string ProductName,
    decimal UnitPrice,
    int Quantity,
    decimal LineTotal
);
