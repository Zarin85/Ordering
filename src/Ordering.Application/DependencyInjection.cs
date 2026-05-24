using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.EventHandlers;
using Ordering.Application.Events;
using Ordering.Application.Orders;
using Ordering.Domain.Orders.Events;

namespace Ordering.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IOrderService, OrderService>();

        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        services.AddScoped<IDomainEventHandler<OrderPlacedDomainEvent>, OrderPlacedEventHandler>();
        services.AddScoped<IDomainEventHandler<OrderConfirmedDomainEvent>, OrderConfirmedEventHandler>();
        services.AddScoped<IDomainEventHandler<OrderCancelledDomainEvent>, OrderCancelledEventHandler>();

        return services;
    }
}
