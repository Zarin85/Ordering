namespace Ordering.Application.Orders.Requests;

public record PlaceOrderRequest(
    Guid CustomerId,
    string Street,
    string City,
    string Country,
    List<OrderItemRequest> Items
);
