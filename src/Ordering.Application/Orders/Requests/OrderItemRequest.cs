namespace Ordering.Application.Orders.Requests;

public record OrderItemRequest(
    Guid ProductId,
    string ProductName,
    decimal UnitPrice,
    int Quantity
);
