using FlightStorageService.Models;

namespace FlightStorageService.Repositories;

public interface IFlightRepository
{
    Flight GetTestFlight();
}