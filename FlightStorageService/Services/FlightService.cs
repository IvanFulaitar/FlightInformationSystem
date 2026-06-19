using FlightStorageService.Models;
using FlightStorageService.Repositories;
using FlightStorageService.Exceptions;

namespace FlightStorageService.Services;

// Сервісний шар містить бізнес-логіку та валідацію рейсів.
public class FlightService : IFlightService
{
    private readonly IFlightRepository _flightRepository;

    // Репозиторій передається через Dependency Injection.
    public FlightService(IFlightRepository flightRepository)
    {
        _flightRepository = flightRepository;
    }

    // Перевіряє номер рейсу та повертає знайдений рейс або null.
    public Flight? GetByNumber(string flightNumber)
    {
        if (string.IsNullOrWhiteSpace(flightNumber))
        {
            throw new ArgumentException("Flight number is required.");
        }

        flightNumber = NormalizeFlightNumber(flightNumber);

        var flight = _flightRepository.GetByNumber(flightNumber);

        if (flight == null)
        {
            throw new FlightNotFoundException("Flight not found.");
        }

        return flight;
    }

    public List<Flight> GetByDate(DateTime date)
    {
        if (date == default)
        {
            throw new ArgumentException("Date is required.");
        }

        return _flightRepository.GetByDate(date);
    }

    public List<Flight> GetByDepartureCityAndDate(string city, DateTime date)
    {
        if (string.IsNullOrWhiteSpace(city))
        {
            throw new ArgumentException("Departure city is required.");
        }

        city = city.Trim();

        return _flightRepository.GetByDepartureCityAndDate(city, date);
    }

    public List<Flight> GetByArrivalCityAndDate(string city, DateTime date)
    {
        if (string.IsNullOrWhiteSpace(city))
        {
            throw new ArgumentException("Arrival city is required.");
        }

        city = city.Trim();

        return _flightRepository.GetByArrivalCityAndDate(city, date);
    }

    public List<Flight> GetByCity(string city)
    {
        if (string.IsNullOrWhiteSpace(city))
        {
            throw new ArgumentException("City is required.");
        }

        return _flightRepository.GetByCity(city.Trim());
    }

    public void AddFlight(Flight flight)
    {
        ValidateFlight(flight, requireFlightNumber: true);

        NormalizeFlight(flight);

        var existingFlight = _flightRepository.GetByNumber(flight.FlightNumber);

        if (existingFlight != null)
        {
            throw new DuplicateFlightException("Flight number already exists.");
        }

        _flightRepository.AddFlight(flight);
    }

    public List<Flight> GetAll()
    {
        return _flightRepository.GetAll();
    }

    public void UpdateFlight(string flightNumber, Flight flight)
    {
        if (string.IsNullOrWhiteSpace(flightNumber))
        {
            throw new ArgumentException("Flight number is required.");
        }

        flightNumber = NormalizeFlightNumber(flightNumber);

        ValidateFlight(flight, requireFlightNumber: false);

        NormalizeFlight(flight);

        var existingFlight = _flightRepository.GetByNumber(flightNumber);

        if (existingFlight == null)
        {
            throw new FlightNotFoundException("Flight not found.");
        }

        _flightRepository.UpdateFlight(flightNumber, flight);
    }

    public void DeleteFlight(string flightNumber)
    {
        if (string.IsNullOrWhiteSpace(flightNumber))
        {
            throw new ArgumentException("Flight number is required.");
        }

        flightNumber = NormalizeFlightNumber(flightNumber);

        var existingFlight = _flightRepository.GetByNumber(flightNumber);

        if (existingFlight == null)
        {
            throw new FlightNotFoundException("Flight not found.");
        }

        _flightRepository.DeleteFlight(flightNumber);
    }

    private static string NormalizeFlightNumber(string flightNumber)
    {
        return flightNumber.Trim().ToUpperInvariant();
    }

    private static void NormalizeFlight(Flight flight)
    {
        if (!string.IsNullOrWhiteSpace(flight.FlightNumber))
        {
            flight.FlightNumber = NormalizeFlightNumber(flight.FlightNumber);
        }

        flight.DepartureAirportCity = flight.DepartureAirportCity.Trim();
        flight.ArrivalAirportCity = flight.ArrivalAirportCity.Trim();
    }

    private static void ValidateFlight(Flight flight, bool requireFlightNumber)
    {
        if (flight == null)
        {
            throw new ArgumentException("Flight is required.");
        }

        if (requireFlightNumber && string.IsNullOrWhiteSpace(flight.FlightNumber))
        {
            throw new ArgumentException("Flight number is required.");
        }

        if (string.IsNullOrWhiteSpace(flight.DepartureAirportCity))
        {
            throw new ArgumentException("Departure airport city is required.");
        }

        if (string.IsNullOrWhiteSpace(flight.ArrivalAirportCity))
        {
            throw new ArgumentException("Arrival airport city is required.");
        }

        if (string.Equals(
                flight.DepartureAirportCity.Trim(),
                flight.ArrivalAirportCity.Trim(),
                StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException("Departure and arrival cities must be different.");
        }

        if (flight.DepartureDateTime == default)
        {
            throw new ArgumentException("Departure date is required.");
        }

        var today = DateTime.Today;
        var maxDate = today.AddDays(7);

        if (flight.DepartureDateTime.Date < today ||
            flight.DepartureDateTime.Date > maxDate)
        {
            throw new ArgumentException("Flight date must be within the next 7 days.");
        }

        if (flight.DurationMinutes <= 0)
        {
            throw new ArgumentException("Duration must be greater than zero.");
        }
    }
}
