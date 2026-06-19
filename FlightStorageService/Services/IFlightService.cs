using FlightStorageService.Models;

namespace FlightStorageService.Services;

public interface IFlightService
{
    Flight? GetByNumber(string flightNumber);

    List<Flight> GetByDate(DateTime date);

    List<Flight> GetByDepartureCityAndDate(string city, DateTime date);

    List<Flight> GetByArrivalCityAndDate(string city, DateTime date);

    List<Flight> GetAll();

    List<Flight> GetByCity(string city);

    void AddFlight(Flight flight);

    void UpdateFlight(string flightNumber, Flight flight);

    void DeleteFlight(string flightNumber);
}