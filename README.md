# Flight Information System

Flight Information System is a .NET solution for storing, managing, and searching flight information.

The solution contains two applications:

- `FlightStorageService` - ASP.NET Core Web API for storing and retrieving flight data.
- `FlightClientApp` - ASP.NET Core MVC client application for searching flights through the API.

## Technologies

- .NET
- ASP.NET Core Web API
- ASP.NET Core MVC
- PostgreSQL
- Npgsql
- Dapper
- Bootstrap

Note: the original database requirement was adjusted by agreement to PostgreSQL with SQL queries executed through Dapper and Npgsql.

## Solution Structure

```text
FlightInformationSystem/
├── FlightStorageService/
│   ├── Controllers/
│   ├── Models/
│   ├── Repositories/
│   ├── Services/
│   └── Database/
├── FlightClientApp/
│   ├── Controllers/
│   ├── Models/
│   ├── Views/
│   └── wwwroot/
└── README.md
```

## FlightStorageService

`FlightStorageService` is the backend API. It stores flight data in PostgreSQL and exposes REST endpoints for creating, updating, deleting, and searching flights.

Main responsibilities:

- Store flight information.
- Validate flight data.
- Search flights by number, date, departure city, or arrival city.
- Execute SQL queries through Dapper and Npgsql.
- Return JSON responses for client applications and Postman.

API base URL:

```text
http://localhost:5294/api/flights
```

Available endpoints:

- `GET /api/flights/all`
- `GET /api/flights/{flightNumber}`
- `GET /api/flights?date=YYYY-MM-DD`
- `GET /api/flights/departure?city=Kyiv&date=YYYY-MM-DD`
- `GET /api/flights/arrival?city=London&date=YYYY-MM-DD`
- `POST /api/flights`
- `PUT /api/flights/{flightNumber}`
- `DELETE /api/flights/{flightNumber}`

## FlightClientApp

`FlightClientApp` is the web client project. The flight search UI is planned and will call `FlightStorageService` through HTTP.

Planned UI features:

- Search flight by flight number.
- Search flights by departure date.
- Search flights by departure city and date.
- Search flights by arrival city and date.
- Display results with flight number, departure time, departure city, arrival city, and duration.

## Database

The project uses PostgreSQL.

Before running the API:

1. Create a PostgreSQL database.
2. Run the database script:

```text
FlightStorageService/Database/schema.sql
```

3. Set the connection string in:

```text
FlightStorageService/appsettings.json
```

Example connection string:

```text
Host=localhost;Port=5432;Database=flights_db;Username=postgres;Password=postgres
```

Example database creation commands:

```powershell
createdb -U postgres flights_db
psql -U postgres -d flights_db -f FlightStorageService/Database/schema.sql
```

You can also create the database manually through pgAdmin and then run `schema.sql` in the query tool.

Important: `schema.sql` recreates the `flights` table. Existing data in that table will be deleted when the script is executed.

The database stores flights with these fields:

- `flight_number`
- `departure_datetime`
- `departure_airport_city`
- `arrival_airport_city`
- `duration_minutes`

Database constraints:

- `flight_number` is the primary key and has a maximum length of 10 characters.
- Required fields are `NOT NULL`.
- `duration_minutes` must be greater than `0`.
- Departure and arrival cities must be different, case-insensitively.
- `departure_datetime` must be within the current date and the next 7 days.

The project uses direct SQL queries through Dapper and Npgsql. Entity Framework is not used.

## How To Run

Restore and build the solution:

```powershell
dotnet restore
dotnet build
```

Run the backend API:

```powershell
dotnet run --project FlightStorageService
```

Run the client application:

```powershell
dotnet run --project FlightClientApp
```

After starting `FlightStorageService`, test the API through Postman or Swagger UI.

The client application is included in the solution, but the flight search UI is still in progress.

A Postman collection is available at:

```text
Postman/FlightInformationSystem.postman_collection.json
```

## Example API Request

Get all flights:

```http
GET /api/flights/all
```

Get a flight by number:

```http
GET /api/flights/PS123
```

Get flights by departure date:

```http
GET /api/flights?date=2026-06-20
```

Get flights by departure city and date:

```http
GET /api/flights/departure?city=Kyiv&date=2026-06-20
```

Get flights by arrival city and date:

```http
GET /api/flights/arrival?city=London&date=2026-06-20
```

Create a flight:

```http
POST /api/flights
Content-Type: application/json
```

```json
{
  "flightNumber": "PS123",
  "departureDateTime": "2026-06-20T10:30:00",
  "departureAirportCity": "Kyiv",
  "arrivalAirportCity": "London",
  "durationMinutes": 180
}
```

## Expected API Responses

- `200 OK` - data was returned successfully.
- `200 OK` with `[]` - no flights were found for a list request.
- `201 Created` - a new flight was created.
- `204 No Content` - a flight was updated or deleted.
- `400 Bad Request` - request data failed validation.
- `404 Not Found` - a single requested flight was not found.
- `409 Conflict` - a flight with the same flight number already exists.
- `500 Internal Server Error` - unexpected server error.

## Postman Checklist

Positive scenarios:

- Create a valid flight.
- Get a flight by flight number.
- Get all flights.
- Get flights by date.
- Get flights by departure city and date.
- Get flights by arrival city and date.
- Update an existing flight.
- Delete an existing flight.

Negative scenarios:

- Create a flight with an empty flight number.
- Create a flight with duplicate flight number.
- Create a flight with `durationMinutes` equal to `0`.
- Create a flight with negative `durationMinutes`.
- Create a flight with the same departure and arrival city.
- Search by an empty city.
- Update a non-existing flight.
- Delete a non-existing flight.
- Request a non-existing flight by number.

Edge cases:

- Search for a date that has no flights.
- Search using different city letter casing.
- Create a flight with leading or trailing spaces.
- Send invalid JSON.

## OpenAPI

OpenAPI JSON is available in development mode:

```text
http://localhost:5294/openapi/v1.json
```

Swagger UI is available in development mode:

```text
http://localhost:5294/swagger
```

## Notes

- Database access is implemented with PostgreSQL, Npgsql, and SQL queries through Dapper.
- Entity Framework is not used.
- Generated build output such as `bin/` and `obj/` should not be committed.
