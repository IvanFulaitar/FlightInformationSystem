using Microsoft.AspNetCore.Mvc;
using FlightStorageService.Models;
using FlightStorageService.Services;

namespace FlightStorageService.Controllers;

[ApiController]
[Route("api/[controller]")]
// Контролер - точка входу для HTTP-запитів.
// Приймає запит від клієнта, викликає сервіс та повертає відповідь.
public class FlightsController : ControllerBase
{
    private readonly IFlightService _flightService;

    public FlightsController(IFlightService flightService)
    {
        _flightService = flightService;
    }

    [HttpGet("test")]
    public IActionResult Test()
    {
        var flight = _flightService.GetTestFlight();

        return Ok(flight);
    }

    [HttpGet("{flightNumber}")]
    public IActionResult GetByNumber(string flightNumber)
    {
        try
        {
            var flight = _flightService.GetByNumber(flightNumber);

            if (flight == null)
            {
                return NotFound();
            }

            return Ok(flight);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpGet]
    public IActionResult GetByDate(DateTime date)
    {
        try
        {
            var flights = _flightService.GetByDate(date);

            if (flights.Count == 0)
            {
                return NotFound();
            }

            return Ok(flights);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpGet("departure")]
    public IActionResult GetByDeparture(string city, DateTime date)
    {
        try
        {
            var flights = _flightService.GetByDepartureCityAndDate(city, date);

            if (flights.Count == 0)
            {
                return NotFound();
            }

            return Ok(flights);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpGet("arrival")]
    public IActionResult GetByArrival(string city, DateTime date)
    {
        try
        {
            var flights = _flightService.GetByArrivalCityAndDate(city, date);

            if (flights.Count == 0)
            {
                return NotFound();
            }

            return Ok(flights);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(exception.Message);
        }
    }
}
