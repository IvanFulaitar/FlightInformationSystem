using FlightStorageService.Models;
using FlightStorageService.Repositories;

namespace FlightStorageService.Services;

public class FlightService : IFlightService
//Містить бізнес-логіку.
//Питання: Що робити?
{
    private readonly IFlightRepository _flightRepository;

    public FlightService(IFlightRepository flightRepository)
    {
        _flightRepository = flightRepository;
    }

    public Flight GetTestFlight()
    {
        return _flightRepository.GetTestFlight();
    }
}