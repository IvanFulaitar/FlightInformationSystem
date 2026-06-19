using FlightClientApp.Models;

namespace FlightClientApp.Services;

public interface IFlightApiClient
{
    Task<List<FlightViewModel>> GetAllAsync();

    Task<FlightViewModel?> GetByNumberAsync(string flightNumber);

    Task<List<FlightViewModel>> GetByDateAsync(DateTime date);

    Task<List<FlightViewModel>> GetByDepartureCityAndDateAsync(string city, DateTime date);

    Task<List<FlightViewModel>> GetByArrivalCityAndDateAsync(string city, DateTime date);

    Task<List<FlightViewModel>> GetByCityAsync(string city);

    Task CreateAsync(FlightViewModel flight);

    Task UpdateAsync(string flightNumber, FlightViewModel flight);

    Task DeleteAsync(string flightNumber);
}
