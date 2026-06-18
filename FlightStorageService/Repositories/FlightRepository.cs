using Dapper;
using Npgsql;
using FlightStorageService.Models;
using FlightStorageService.Exceptions;

namespace FlightStorageService.Repositories;

// Репозиторій відповідає за доступ до даних.
// Працює з PostgreSQL через Npgsql + Dapper.
public class FlightRepository : IFlightRepository
{
    private readonly string _connectionString;

    public FlightRepository(IConfiguration configuration)
    {
        _connectionString =
            configuration.GetConnectionString("FlightsDb")
            ?? throw new InvalidOperationException(
                "Connection string 'FlightsDb' not found.");
    }

    public Flight? GetByNumber(string flightNumber)
    {
        using var connection = new NpgsqlConnection(_connectionString);

        const string sql = """
            SELECT
                flight_number AS FlightNumber,
                departure_datetime AS DepartureDateTime,
                departure_airport_city AS DepartureAirportCity,
                arrival_airport_city AS ArrivalAirportCity,
                duration_minutes AS DurationMinutes
            FROM flights
            WHERE flight_number = @FlightNumber;
            """;

        return connection.QuerySingleOrDefault<Flight>(
            sql,
            new { FlightNumber = flightNumber });
    }

    public List<Flight> GetByDate(DateTime date)
    {
        using var connection = new NpgsqlConnection(_connectionString);

        const string sql = """
            SELECT
                flight_number AS FlightNumber,
                departure_datetime AS DepartureDateTime,
                departure_airport_city AS DepartureAirportCity,
                arrival_airport_city AS ArrivalAirportCity,
                duration_minutes AS DurationMinutes
            FROM flights
            WHERE departure_datetime::date = @Date
            ORDER BY departure_datetime;
            """;

        return connection.Query<Flight>(
            sql,
            new { Date = date.Date })
            .ToList();
    }

    public List<Flight> GetByDepartureCityAndDate(string city, DateTime date)
    {
        using var connection = new NpgsqlConnection(_connectionString);

        const string sql = """
            SELECT
                flight_number AS FlightNumber,
                departure_datetime AS DepartureDateTime,
                departure_airport_city AS DepartureAirportCity,
                arrival_airport_city AS ArrivalAirportCity,
                duration_minutes AS DurationMinutes
            FROM flights
            WHERE departure_airport_city = @City
              AND departure_datetime::date = @Date
            ORDER BY departure_datetime;
            """;

        return connection.Query<Flight>(
            sql,
            new
            {
                City = city,
                Date = date.Date
            })
            .ToList();
    }

    public List<Flight> GetByArrivalCityAndDate(string city, DateTime date)
    {
        using var connection = new NpgsqlConnection(_connectionString);

        const string sql = """
            SELECT
                flight_number AS FlightNumber,
                departure_datetime AS DepartureDateTime,
                departure_airport_city AS DepartureAirportCity,
                arrival_airport_city AS ArrivalAirportCity,
                duration_minutes AS DurationMinutes
            FROM flights
            WHERE arrival_airport_city = @City
              AND departure_datetime::date = @Date
            ORDER BY departure_datetime;
            """;

        return connection.Query<Flight>(
            sql,
            new
            {
                City = city,
                Date = date.Date
            })
            .ToList();
    }

    public List<Flight> GetAll()
    {
        using var connection = new NpgsqlConnection(_connectionString);

        const string sql = """
            SELECT
                flight_number AS FlightNumber,
                departure_datetime AS DepartureDateTime,
                departure_airport_city AS DepartureAirportCity,
                arrival_airport_city AS ArrivalAirportCity,
                duration_minutes AS DurationMinutes
            FROM flights
            ORDER BY departure_datetime;
            """;

        return connection.Query<Flight>(sql).ToList();
    }

    public void AddFlight(Flight flight)
    {
        using var connection = new NpgsqlConnection(_connectionString);

        const string sql = """
            INSERT INTO flights
            (
                flight_number,
                departure_datetime,
                departure_airport_city,
                arrival_airport_city,
                duration_minutes
            )
            VALUES
            (
                @FlightNumber,
                @DepartureDateTime,
                @DepartureAirportCity,
                @ArrivalAirportCity,
                @DurationMinutes
            );
            """;

        try
        {
            connection.Execute(sql, flight);
        }
        catch (PostgresException exception)
            when (exception.SqlState == PostgresErrorCodes.UniqueViolation)
        {
            throw new DuplicateFlightException("Flight number already exists.");
        }
    }

    public void UpdateFlight(string flightNumber, Flight flight)
    {
        using var connection = new NpgsqlConnection(_connectionString);

        const string sql = """
            UPDATE flights
            SET departure_datetime = @DepartureDateTime,
                departure_airport_city = @DepartureAirportCity,
                arrival_airport_city = @ArrivalAirportCity,
                duration_minutes = @DurationMinutes
            WHERE flight_number = @FlightNumber;
            """;

        var rowsAffected = connection.Execute(
            sql,
            new
            {
                FlightNumber = flightNumber,
                flight.DepartureDateTime,
                flight.DepartureAirportCity,
                flight.ArrivalAirportCity,
                flight.DurationMinutes
            });

        if (rowsAffected == 0)
        {
            throw new KeyNotFoundException("Flight not found.");
        }
    }

    public void DeleteFlight(string flightNumber)
    {
        using var connection = new NpgsqlConnection(_connectionString);

        const string sql = """
            DELETE FROM flights
            WHERE flight_number = @FlightNumber;
            """;

        var rowsAffected = connection.Execute(
            sql,
            new { FlightNumber = flightNumber });

        if (rowsAffected == 0)
        {
            throw new KeyNotFoundException("Flight not found.");
        }
    }
}