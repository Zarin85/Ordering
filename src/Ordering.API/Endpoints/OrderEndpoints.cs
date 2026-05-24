using Ordering.Application.Orders;
using Ordering.Application.Orders.Requests;

namespace Ordering.API.Endpoints;

public static class OrderEndpoints
{
    public static void MapOrderEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/orders").WithTags("Orders");

        group.MapPost("/", async (PlaceOrderRequest request, IOrderService service, CancellationToken ct) =>
        {
            var id = await service.PlaceOrderAsync(request, ct);
            return Results.Created($"/api/orders/{id}", new { id });
        })
        .WithName("PlaceOrder");

        group.MapGet("/", async (IOrderService service, CancellationToken ct) =>
        {
            var orders = await service.GetAllAsync(ct);
            return Results.Ok(orders);
        })
        .WithName("GetAllOrders");

        group.MapGet("/{id:guid}", async (Guid id, IOrderService service, CancellationToken ct) =>
        {
            var order = await service.GetByIdAsync(id, ct);
            return order is null ? Results.NotFound() : Results.Ok(order);
        })
        .WithName("GetOrderById");

        group.MapPut("/{id:guid}/confirm", async (Guid id, IOrderService service, CancellationToken ct) =>
        {
            await service.ConfirmOrderAsync(id, ct);
            return Results.NoContent();
        })
        .WithName("ConfirmOrder");

        group.MapPut("/{id:guid}/cancel", async (Guid id, IOrderService service, CancellationToken ct) =>
        {
            await service.CancelOrderAsync(id, ct);
            return Results.NoContent();
        })
        .WithName("CancelOrder");
    }
}
