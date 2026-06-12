using FlightStorageService.Models;

namespace FlightStorageService.Repositories;
// Репозиторій відповідає за доступ до даних.
// Поки що працюємо з тестовими даними.
// Пізніше тут буде ADO.NET + SQL Server.

public class FlightRepository : IFlightRepository
{
    private readonly List<Flight> _flights =
[
    new Flight
    {
        FlightNumber = "PS123",
        DepartureDateTime = DateTime.Today.AddHours(10),
        DepartureAirportCity = "Kyiv",
        ArrivalAirportCity = "London",
        DurationMinutes = 180
    },

    new Flight
    {
        FlightNumber = "PS456",
        DepartureDateTime = DateTime.Today.AddHours(14),
        DepartureAirportCity = "Kyiv",
        ArrivalAirportCity = "Paris",
        DurationMinutes = 150
    },

    new Flight
    {
        FlightNumber = "PS789",
        DepartureDateTime = DateTime.Today.AddDays(1).AddHours(9),
        DepartureAirportCity = "Warsaw",
        ArrivalAirportCity = "London",
        DurationMinutes = 120
    },

    new Flight
    {
        FlightNumber = "PS999",
        DepartureDateTime = DateTime.Today.AddDays(2).AddHours(16),
        DepartureAirportCity = "Prague",
        ArrivalAirportCity = "Rome",
        DurationMinutes = 140
    }
];

    public Flight GetTestFlight()
    {
        return new Flight
        {
            FlightNumber = "PS123",
            DepartureDateTime = DateTime.Now.AddHours(2),
            DepartureAirportCity = "Kyiv",
            ArrivalAirportCity = "London",
            DurationMinutes = 180
        };
    }

    public Flight? GetByNumber(string flightNumber) 
    {
        return _flights.FirstOrDefault(
        flight => flight.FlightNumber.Equals(
            flightNumber,
            StringComparison.OrdinalIgnoreCase
        )
    );
   );
    }

    public List<Flight> GetByDate(DateTime date)
    {
        return _flights.Where(flight => flight.DepartureDateTime.Date == date.Date).ToList();
    }

    public List<Flight> GetByDepartureCityAndDate(string city,DateTime date)
    {
        return _flights
        .Where(flight =>
            flight.DepartureAirportCity.Equals(city, StringComparison.OrdinalIgnoreCase) &&
            flight.DepartureDateTime.Date == date.Date)
        .ToList();
    }

    public List<Flight> GetByArrivalCityAndDate(string city, DateTime date)
    {
        return _flights
        .Where(flight =>
            flight.ArrivalAirportCity.Equals(city, StringComparison.OrdinalIgnoreCase) &&
            flight.DepartureDateTime.Date == date.Date)
        .ToList();
    }
}