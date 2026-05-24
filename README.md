# Ordering — Pure DDD Learning Project

A hands-on project to learn **Domain-Driven Design (DDD)** building blocks in .NET 10, built as a contrast to a Clean Architecture project.

## Goals

- Practice DDD aggregates, value objects, domain events, and invariants
- Understand how the repository pattern lives in the Domain layer
- Build a working Ordering API without MediatR — using plain application service classes

## Architecture

```
Ordering.Domain          # Aggregates, Value Objects, Domain Events, IOrderRepository
Ordering.Application     # IOrderService, domain event handlers, FluentValidation
Ordering.Infrastructure  # EF Core + SQLite, OrderRepository, DomainEventDispatcher
Ordering.API             # Minimal API endpoints, GlobalExceptionHandler, Swagger
```

**Dependency flow:** API → Application + Infrastructure → Domain

## Key Design Decisions

- **No MediatR** — `IOrderService` is injected directly into endpoints
- **Repository interface in Domain** — `IOrderRepository` lives in `Ordering.Domain`
- **Invariants in the aggregate** — `Order` throws `DomainException` for invalid state
- **Domain events dispatched after `SaveChangesAsync`** — via `DomainEventDispatcher` resolving `IDomainEventHandler<T>` from `IServiceProvider`

## Tech Stack

- .NET 10
- Entity Framework Core + SQLite
- FluentValidation
- Swashbuckle (Swagger)

## Running Locally

```bash
dotnet restore
dotnet ef database update --project src/Ordering.Infrastructure --startup-project src/Ordering.API
dotnet run --project src/Ordering.API
```

Swagger UI available at `https://localhost:{port}/swagger`.
