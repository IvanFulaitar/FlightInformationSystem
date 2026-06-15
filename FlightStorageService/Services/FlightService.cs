using FlightStorageService.Models;
using FlightStorageService.Repositories;

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

        return _flightRepository.GetByNumber(flightNumber);
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

        return _flightRepository.GetByDepartureCityAndDate(city, date);
    }

    public List<Flight> GetByArrivalCityAndDate(string city, DateTime date)
    {
        if (string.IsNullOrWhiteSpace(city))
        {
            throw new ArgumentException("Arrival city is required.");
        }

        return _flightRepository.GetByArrivalCityAndDate(city, date);
    }

    public void AddFlight(Flight flight)
    {
        ValidateFlight(flight, requireFlightNumber: true);

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

        ValidateFlight(flight, requireFlightNumber: false);

        var existingFlight = _flightRepository.GetByNumber(flightNumber);

        if (existingFlight == null)
        {
            throw new KeyNotFoundException("Flight not found.");
        }

        _flightRepository.UpdateFlight(flightNumber, flight);
    }

    public void DeleteFlight(string flightNumber)
    {
        if (string.IsNullOrWhiteSpace(flightNumber))
        {
            throw new ArgumentException("Flight number is required.");
        }

        var existingFlight = _flightRepository.GetByNumber(flightNumber);

        if (existingFlight == null)
        {
            throw new KeyNotFoundException("Flight not found.");
        }

        _flightRepository.DeleteFlight(flightNumber);
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

        if (flight.DepartureDateTime == default)
        {
            throw new ArgumentException("Departure date is required.");
        }

        if (flight.DurationMinutes <= 0)
        {
            throw new ArgumentException("Duration must be greater than zero.");
        }
    }
}
