# FlightInformationSystem

FlightInformationSystem is a .NET solution for storing and retrieving flight information.

## Projects

- `FlightClientApp` - ASP.NET Core MVC client application.
- `FlightStorageService` - ASP.NET Core Web API for flight data storage.

## FlightStorageService

The storage service exposes HTTP endpoints for working with flights stored in PostgreSQL.

### Tech Stack

- .NET 10
- ASP.NET Core Web API
- PostgreSQL
- Npgsql

### Configuration

The PostgreSQL connection string is configured in:

```text
FlightStorageService/appsettings.json
```

Default connection string:

```text
Host=localhost;Port=5432;Database=flights_db;Username=postgres;Password=postgres
```

The service expects a `flights` table with these columns:

```sql
flight_number text
departure_datetime timestamp
departure_airport_city text
arrival_airport_city text
duration_minutes integer
```

### API Endpoints

Base route:

```text
http://localhost:5294/api/flights
```

Available endpoints:

- `GET /api/flights/all` - get all flights.
- `GET /api/flights/{flightNumber}` - get a flight by flight number.
- `GET /api/flights?date=2026-06-15` - get flights by departure date.
- `GET /api/flights/departure?city=Kyiv&date=2026-06-15` - get flights by departure city and date.
- `GET /api/flights/arrival?city=London&date=2026-06-15` - get flights by arrival city and date.
- `POST /api/flights` - create a new flight.
- `PUT /api/flights/{flightNumber}` - update an existing flight.
- `DELETE /api/flights/{flightNumber}` - delete a flight.

Example request body for `POST`:

```json
{
  "flightNumber": "PS123",
  "departureDateTime": "2026-06-15T10:30:00",
  "departureAirportCity": "Kyiv",
  "arrivalAirportCity": "London",
  "durationMinutes": 180
}
```

Example request body for `PUT /api/flights/PS123`:

```json
{
  "departureDateTime": "2026-06-15T10:30:00",
  "departureAirportCity": "Kyiv",
  "arrivalAirportCity": "London",
  "durationMinutes": 180
}
```

### Expected Responses

- `200 OK` - data was found and returned.
- `201 Created` - a new flight was created.
- `204 No Content` - a flight was updated or deleted.
- `400 Bad Request` - request data failed validation.
- `404 Not Found` - no matching flight was found.

### Postman Test Checklist

Positive cases:

- `POST /api/flights` - create a flight.
- `GET /api/flights/{flightNumber}` - get the created flight by number.
- `GET /api/flights/all` - get all flights.
- `GET /api/flights?date=2026-06-15` - get flights by date.
- `GET /api/flights/departure?city=Kyiv&date=2026-06-15` - get flights by departure city and date.
- `GET /api/flights/arrival?city=London&date=2026-06-15` - get flights by arrival city and date.
- `PUT /api/flights/{flightNumber}` - update a flight.
- `DELETE /api/flights/{flightNumber}` - delete a flight.

Negative cases:

- `POST /api/flights` with an empty `flightNumber` should return `400 Bad Request`.
- `POST /api/flights` with `durationMinutes: 0` should return `400 Bad Request`.
- `PUT /api/flights/UNKNOWN` should return `404 Not Found`.
- `DELETE /api/flights/UNKNOWN` should return `404 Not Found`.
- `GET /api/flights?date=2099-01-01` should return `404 Not Found` when there are no flights for that date.

OpenAPI JSON is available in development mode:

```text
http://localhost:5294/openapi/v1.json
```

## Getting Started

Restore and build:

```powershell
dotnet restore
dotnet build
```

Run the storage service:

```powershell
dotnet run --project FlightStorageService
```

Run the client application:

```powershell
dotnet run --project FlightClientApp
```

OpenAPI is available in development mode at the URL exposed by ASP.NET Core for the service.

## Git

The repository ignores local IDE files and generated build output such as `.vs/`, `bin/`, and `obj/`.
