# Hotel Reservation Management API

A REST API for managing hotels, customers and reservations, built with .NET 9 following Clean Architecture principles.

---

## Requirements

| Tool | Version | Needed for |
|---|---|---|
| Docker Desktop | latest | Running the app (recommended) |
| .NET SDK | 9.0 | Running locally without Docker |
| PostgreSQL | 16 | Running locally without Docker |

---

## Running the application

### Option 1 — Docker Compose (recommended)

Requires only Docker Desktop. No .NET SDK or PostgreSQL installation needed.

```
git clone https://github.com/AngelTsounis/Hotel-Reservation-Management-API
cd Hotel-Reservation-Management-API
docker compose up --build
```

Swagger UI: http://localhost:8080/swagger

This starts two containers:

| Container | Description | Port |
|---|---|---|
| `hotel-reservation-api` | .NET 9 API | 8080 |
| `hotel-reservation-db` | PostgreSQL 16 | 5433 (host) |

Database migrations are applied automatically on startup.

To stop:

```
docker compose down      # keeps database data
docker compose down -v   # also removes database data
```

### Option 2 — Running locally

1. Clone the repository:

```
git clone https://github.com/AngelTsounis/Hotel-Reservation-Management-API
cd Hotel-Reservation-Management-API
```

2. Create a PostgreSQL database.

3. Set the connection string using user secrets (it is intentionally empty in `appsettings.json` so no credentials are committed):

```
cd Hotel.Reservation.Management
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=HotelReservationManagement;Username=postgres;Password=yourpassword"
```

4. Run the API:

```
dotnet run --project Hotel.Reservation.Management
```

Swagger UI: https://localhost:7238/swagger

## Database configuration

PostgreSQL, accessed via Entity Framework Core 9.

The connection string is read from `ConnectionStrings:DefaultConnection`. Configuration sources, in order of precedence:

1. Environment variables (`ConnectionStrings__DefaultConnection`) — used by Docker Compose
2. User secrets — used for local development
3. `appsettings.json` — intentionally left empty

Migrations run automatically at startup via `Database.MigrateAsync()`, so no manual `dotnet ef database update` is required.

### Schema

| Table | Notes |
|---|---|
| `Hotels` | Indexed on `City` |
| `Customers` | Unique index on `Email` |
| `Reservations` | FKs to `Hotels` and `Customers` with `RESTRICT` delete behaviour; composite index on `(CustomerId, Status)` |

`Reservations.Status` is persisted as a string (`ACTIVE` / `CANCELLED`) rather than an integer.

---

## Running the tests

### Unit tests

No external dependencies.

```
dotnet test Hotel.Reservation.Management.UnitTests
```

Covers domain entities (business rules, overlap detection, cancellation), FluentValidation validators, and application services with mocked repositories.

### Integration tests

**Requires Docker Desktop to be running.** A throwaway PostgreSQL container is started automatically via Testcontainers, migrated, and destroyed after the run — your local database is never touched.

```
dotnet test Hotel.Reservation.Management.IntegrationTests
```

Covers repository behaviour against real PostgreSQL: dynamic search filtering, case-insensitive matching, logical cancellation persistence and email uniqueness.

### All tests

```
dotnet test
```

---

## API examples

The database starts **empty**. Create a hotel and a customer before creating reservations.

### Create a hotel

```
POST /api/hotels
Content-Type: application/json

{
  "name": "Hilton Athens",
  "city": "Athens",
  "stars": 5
}
```

### Create a customer

```
POST /api/customer
Content-Type: application/json

{
  "firstName": "Angel",
  "lastName": "Tsounis",
  "email": "angel.tsounis@hotmail.com"
}
```

### Create a reservation

```
POST /api/reservations
Content-Type: application/json

{
  "hotelId": 1,
  "customerId": 1,
  "checkInDate": "2026-08-10T00:00:00Z",
  "checkOutDate": "2026-08-15T00:00:00Z",
  "totalPrice": 750.00
}
```

Response:

```
{
  "id": 1,
  "hotelId": 1,
  "customerId": 1,
  "checkInDate": "2026-08-10T00:00:00",
  "checkOutDate": "2026-08-15T00:00:00",
  "totalPrice": 750.00,
  "status": "Active"
}
```

### Cancel a reservation

```
DELETE /api/reservations/1
```

Performs a **logical cancellation** — the record is retained with status `Cancelled` rather than being removed.

### Search reservations

All parameters are optional and can be combined.

```
GET /api/search/search?hotelName=hilton&city=athens&status=Active
```

| Parameter | Type | Description |
|---|---|---|
| `hotelName` | string | Partial, case-insensitive |
| `customerName` | string | Matches first **or** last name, partial, case-insensitive |
| `city` | string | Partial, case-insensitive |
| `status` | enum | `Active` or `Cancelled` |
| `checkIn` | date | Reservations starting on or after this date |
| `checkOut` | date | Reservations ending on or before this date |

Response includes resolved hotel and customer names alongside the IDs.

---

## Error responses

| Status | Cause |
|---|---|
| `400 Bad Request` | Validation failure, business rule violation, or malformed JSON |
| `404 Not Found` | Referenced hotel, customer or reservation does not exist |
| `409 Conflict` | Duplicate customer email, or overlapping reservation dates for the same customer |
| `500 Internal Server Error` | Unhandled exception (details are not exposed to the client) |

Example:

```
{
  "status": 409,
  "errors": ["Customer with ID 1 already has a reservation overlapping the requested dates."]
}
```

---

## Architecture

```
Hotel.Reservation.Management                 API — minimal API endpoints, filters, exception handling
Hotel.Reservation.Management.Application     Services, DTOs, mapping, validators, repository interfaces
Hotel.Reservation.Management.Domain          Entities, enums, domain exceptions
Hotel.Reservation.Management.Infrastructure  EF Core, repositories, configurations, migrations
```

Dependencies point inwards: `API → Application → Domain`, with `Infrastructure` implementing the interfaces defined in `Application`. The domain layer has no external dependencies.

Business rules are enforced as invariants inside the domain entities, so an entity cannot be constructed in an invalid state regardless of which layer creates it.

---


## SQL Challenges

Standalone SQL exercises / challenges are in `docs/SQL Queries/SQL-Queries.sql`.
