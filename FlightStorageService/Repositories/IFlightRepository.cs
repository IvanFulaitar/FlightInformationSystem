using FlightStorageService.Models;

namespace FlightStorageService.Repositories;

public interface IFlightRepository
{
    Flight GetTestFlight();

    Flight? GetByNumber(string flightNumber);

    List<Flight> GetByDate(DateTime date);

    List<Flight> GetByDepartureCityAndDate(string city, DateTime date);

    List<Flight> GetByArrivalCityAndDate(string city, DateTime date);
}