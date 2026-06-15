using FlightStorageService.Models;

namespace FlightStorageService.Repositories;

public interface IFlightRepository
{
    Flight? GetByNumber(string flightNumber);

    List<Flight> GetByDate(DateTime date);

    List<Flight> GetByDepartureCityAndDate(string city, DateTime date);

    List<Flight> GetByArrivalCityAndDate(string city, DateTime date);

    List<Flight> GetAll();

    void AddFlight(Flight flight);

    void UpdateFlight(string flightNumber, Flight flight);

    void DeleteFlight(string flightNumber);
}