using FlightStorageService.Models;

namespace FlightStorageService.Repositories;

public class FlightRepository : IFlightRepository
//Тільки працює з даними.
//ЗВІДКИ взяти дані?
{
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
}