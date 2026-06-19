using FlightStorageService.Exceptions;
using FlightStorageService.Models;
using FlightStorageService.Repositories;
using FlightStorageService.Services;
using Xunit;

namespace FlightStorageService.Tests;

public class FlightServiceTests
{
    [Fact]
    public void AddFlight_WithEmptyFlightNumber_ThrowsArgumentException()
    {
        var service = new FlightService(new FakeFlightRepository());
        var flight = CreateValidFlight();
        flight.FlightNumber = "";

        Assert.Throws<ArgumentException>(() => service.AddFlight(flight));
    }

    [Fact]
    public void AddFlight_WithDuplicateFlightNumber_ThrowsDuplicateFlightException()
    {
        var repository = new FakeFlightRepository
        {
            ExistingFlight = CreateValidFlight()
        };
        var service = new FlightService(repository);

        Assert.Throws<DuplicateFlightException>(() => service.AddFlight(CreateValidFlight()));
    }

    [Fact]
    public void AddFlight_WithSameCities_ThrowsArgumentException()
    {
        var service = new FlightService(new FakeFlightRepository());
        var flight = CreateValidFlight();
        flight.ArrivalAirportCity = "kyiv";

        Assert.Throws<ArgumentException>(() => service.AddFlight(flight));
    }

    [Fact]
    public void AddFlight_WithZeroDuration_ThrowsArgumentException()
    {
        var service = new FlightService(new FakeFlightRepository());
        var flight = CreateValidFlight();
        flight.DurationMinutes = 0;

        Assert.Throws<ArgumentException>(() => service.AddFlight(flight));
    }

    [Fact]
    public void AddFlight_WithDateOutsideNextSevenDays_ThrowsArgumentException()
    {
        var service = new FlightService(new FakeFlightRepository());
        var flight = CreateValidFlight();
        flight.DepartureDateTime = DateTime.Today.AddDays(8);

        Assert.Throws<ArgumentException>(() => service.AddFlight(flight));
    }

    [Fact]
    public void GetByCity_WithEmptyCity_ThrowsArgumentException()
    {
        var service = new FlightService(new FakeFlightRepository());

        Assert.Throws<ArgumentException>(() => service.GetByCity(" "));
    }

    [Fact]
    public void GetByCity_WithValidCity_TrimsCityAndCallsRepository()
    {
        var repository = new FakeFlightRepository();
        var service = new FlightService(repository);

        service.GetByCity(" Kyiv ");

        Assert.Equal("Kyiv", repository.RequestedCity);
    }
    [Fact]
    public void AddFlight_WithValidFlight_CallsRepository()
    {
        var repository = new FakeFlightRepository();
        var service = new FlightService(repository);
        var flight = CreateValidFlight();
        flight.FlightNumber = " ps123 ";

        service.AddFlight(flight);

        Assert.True(repository.AddFlightWasCalled);
        Assert.Equal("PS123", repository.AddedFlight?.FlightNumber);
    }

    private static Flight CreateValidFlight()
    {
        return new Flight
        {
            FlightNumber = "PS123",
            DepartureDateTime = DateTime.Today.AddDays(1).AddHours(10),
            DepartureAirportCity = "Kyiv",
            ArrivalAirportCity = "London",
            DurationMinutes = 180
        };
    }

    private class FakeFlightRepository : IFlightRepository
    {
        public Flight? ExistingFlight { get; set; }

        public bool AddFlightWasCalled { get; private set; }

        public Flight? AddedFlight { get; private set; }

        public string? RequestedCity { get; private set; }

        public Flight? GetByNumber(string flightNumber)
        {
            return ExistingFlight;
        }

        public List<Flight> GetByDate(DateTime date)
        {
            return new List<Flight>();
        }

        public List<Flight> GetByDepartureCityAndDate(string city, DateTime date)
        {
            return new List<Flight>();
        }

        public List<Flight> GetByArrivalCityAndDate(string city, DateTime date)
        {
            return new List<Flight>();
        }

        public List<Flight> GetAll()
        {
            return new List<Flight>();
        }

        public List<Flight> GetByCity(string city)
        {
            RequestedCity = city;
            return new List<Flight>();
        }
        public void AddFlight(Flight flight)
        {
            AddFlightWasCalled = true;
            AddedFlight = flight;
        }

        public void UpdateFlight(string flightNumber, Flight flight)
        {
        }

        public void DeleteFlight(string flightNumber)
        {
        }
    }
}



