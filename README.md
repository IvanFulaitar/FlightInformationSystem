# FlightInformationSystem

FlightInformationSystem is a .NET solution for a flight information system.

## Projects

- `FlightClientApp` - ASP.NET Core MVC client application.
- `FlightStorageService` - ASP.NET Core service for flight data.

## Getting Started

Restore and build the solution:

```powershell
dotnet restore
dotnet build
```

Run a project:

```powershell
dotnet run --project FlightStorageService
dotnet run --project FlightClientApp
```

## Git

The repository ignores local IDE files and generated build output such as `.vs/`, `bin/`, and `obj/`.
