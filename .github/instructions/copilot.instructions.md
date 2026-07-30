---
applyTo: "**/*.cs"
---

# Hotel Reservation Management — C# conventions

## Architecture

The solution uses .NET 9, Minimal APIs, EF Core, PostgreSQL, Clean Architecture, and DDD.

Dependencies flow inward:

- API → Application, Infrastructure, Domain
- Infrastructure → Application, Domain
- Application → Domain
- Domain → nothing

Do not reference EF Core or ASP.NET Core from Domain or Application.

## Domain

Entities live in `Domain.Model`, inherit `BaseEntity`, and contain business logic.

- Use public getters and private setters.
- Include a private parameterless constructor for EF Core.
- Create entities through public constructors that validate and normalize input.
- Change state through domain methods such as `ReservationEntity.Cancel()`.
- Throw `BusinessRuleException` for violated invariants.
- Do not use data annotations.

Rules requiring database access, such as email uniqueness and reservation overlap checks, are coordinated by Application services.

## Application

- Services implement interfaces from `Services/Interfaces`.
- Services depend only on repository interfaces from `Application/Interfaces`.
- Services return response contracts, never domain entities.
- Use hand-written mapping extensions; do not introduce AutoMapper.
- Use FluentValidation with explicit error messages.
- Throw `NotFoundException`, `ConflictException`, or `BusinessRuleException`; do not return error DTOs.

Every async method ends with `Async`, accepts `CancellationToken cancellationToken = default` last, and forwards it downstream.

Return collections as `IReadOnlyList<T>`.

## Infrastructure

- Put EF mappings in `Persistence/Configurations` using `IEntityTypeConfiguration<T>`.
- Repository implementations inject `AppDbContext`.
- Read queries use `AsNoTracking()` and deterministic ordering.
- Repositories return nullable entities for missing records; services throw not-found exceptions.
- Build dynamic searches with one progressively filtered `IQueryable`.
- Register repositories as scoped in `AddInfrastructure`.

## Minimal APIs

Place one endpoint per file under `Endpoints/<Feature>`.

Each endpoint:

- Is a static class.
- Defines a `Name` constant.
- Exposes a `MapTo...` extension method.
- Uses endpoint validation filters for request bodies.
- Declares response metadata with `.Produces...()`.

Handlers must only bind input, call a service, and return `Results`.

Do not access EF Core, apply business rules, or add try/catch blocks inside endpoints. Errors are handled by `GlobalExceptionHandler`.

## Adding a feature

1. Add or update domain behavior.
2. Add EF Core configuration.
3. Add repository interface and implementation.
4. Add contracts, mapping, and validation.
5. Add service interface and implementation.
6. Register dependencies.
7. Add and register the endpoint.
8. Add focused tests.