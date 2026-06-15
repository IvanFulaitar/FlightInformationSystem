using Npgsql;
using FlightStorageService.Models;

namespace FlightStorageService.Repositories;

// Репозиторій відповідає за доступ до даних.
// Працює з PostgreSQL через Npgsql.
public class FlightRepository : IFlightRepository
{
    private static Flight MapFlight(NpgsqlDataReader reader)
    {
        return new Flight
        {
            FlightNumber = reader.GetString(0),
            DepartureDateTime = reader.GetDateTime(1),
            DepartureAirportCity = reader.GetString(2),
            ArrivalAirportCity = reader.GetString(3),
            DurationMinutes = reader.GetInt32(4)
        };
    }

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

        connection.Open();

        using var command = new NpgsqlCommand(
            """
        SELECT flight_number,
               departure_datetime,
               departure_airport_city,
               arrival_airport_city,
               duration_minutes
        FROM flights
        WHERE flight_number = @flightNumber;
        """,
            connection);

        command.Parameters.AddWithValue("@flightNumber", flightNumber);

        using var reader = command.ExecuteReader();

        if (reader.Read())
        {
            return MapFlight(reader);
        }

        return null;
    }

    public List<Flight> GetByDate(DateTime date)
    {
        var flights = new List<Flight>();

        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        using var command = new NpgsqlCommand(
            """
        SELECT flight_number,
               departure_datetime,
               departure_airport_city,
               arrival_airport_city,
               duration_minutes
        FROM flights
        WHERE departure_datetime::date = @date;
        """,
            connection);

        command.Parameters.AddWithValue("@date", date.Date);

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            flights.Add(MapFlight(reader));
        }

        return flights;
    }

    public List<Flight> GetByDepartureCityAndDate(string city, DateTime date)
    {
        var flights = new List<Flight>();

        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        using var command = new NpgsqlCommand(
       """
        SELECT flight_number,
               departure_datetime,
               departure_airport_city,
               arrival_airport_city,
               duration_minutes
        FROM flights
        WHERE departure_airport_city = @city
          AND departure_datetime::date = @date;
        """,
       connection);

        command.Parameters.AddWithValue("@city", city);
        command.Parameters.AddWithValue("@date", date.Date);

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            flights.Add(MapFlight(reader));
        }

        return flights;

    }

    public List<Flight> GetByArrivalCityAndDate(string city, DateTime date)
    {
        var flights = new List<Flight>();

        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        using var command = new NpgsqlCommand(
       """
        SELECT flight_number,
               departure_datetime,
               departure_airport_city,
               arrival_airport_city,
               duration_minutes
        FROM flights
        WHERE arrival_airport_city = @city
          AND departure_datetime::date = @date;
        """,
       connection);

        command.Parameters.AddWithValue("@city", city);
        command.Parameters.AddWithValue("@date", date.Date);

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            flights.Add(MapFlight(reader));
        }

        return flights;
    }

    public List<Flight> GetAll()
    {
        var flights = new List<Flight>();
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        using var command = new NpgsqlCommand(
        """
         SELECT flight_number,
               departure_datetime,
               departure_airport_city,
               arrival_airport_city,
               duration_minutes
        FROM flights
        ORDER BY departure_datetime;
        """,
        connection);

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            flights.Add(MapFlight(reader));
        }

        return flights;

    }


    public void AddFlight(Flight flight)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        using var command = new NpgsqlCommand(
        """
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
            @flightNumber,
            @departureDateTime,
            @departureAirportCity,
            @arrivalAirportCity,
            @durationMinutes
        );
        """,
        connection);

        command.Parameters.AddWithValue("@flightNumber", flight.FlightNumber);
        command.Parameters.AddWithValue("@departureDateTime", flight.DepartureDateTime);
        command.Parameters.AddWithValue("@departureAirportCity", flight.DepartureAirportCity);
        command.Parameters.AddWithValue("@arrivalAirportCity", flight.ArrivalAirportCity);
        command.Parameters.AddWithValue("@durationMinutes", flight.DurationMinutes);

        command.ExecuteNonQuery();
    }

    public void UpdateFlight(string flightNumber, Flight flight)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        using var command = new NpgsqlCommand(
            """
        UPDATE flights
        SET departure_datetime = @departureDateTime,
            departure_airport_city = @departureAirportCity,
            arrival_airport_city = @arrivalAirportCity,
            duration_minutes = @durationMinutes
        WHERE flight_number = @flightNumber;
        """,
            connection);

        command.Parameters.AddWithValue("@flightNumber", flightNumber);
        command.Parameters.AddWithValue("@departureDateTime", flight.DepartureDateTime);
        command.Parameters.AddWithValue("@departureAirportCity", flight.DepartureAirportCity);
        command.Parameters.AddWithValue("@arrivalAirportCity", flight.ArrivalAirportCity);
        command.Parameters.AddWithValue("@durationMinutes", flight.DurationMinutes);

        var rowsAffected = command.ExecuteNonQuery();

        if (rowsAffected == 0)
        {
            throw new KeyNotFoundException("Flight not found.");
        }
    }

    public void DeleteFlight(string flightNumber)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        using var command = new NpgsqlCommand(
            """
        DELETE FROM flights
        WHERE flight_number = @flightNumber;
        """,
            connection);

        command.Parameters.AddWithValue("@flightNumber", flightNumber);

        var rowsAffected = command.ExecuteNonQuery();

        if (rowsAffected == 0)
        {
            throw new KeyNotFoundException("Flight not found.");
        }
    }
}
