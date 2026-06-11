
using Microsoft.AspNetCore.Mvc;
using FlightStorageService.Models;
using FlightStorageService.Services;

namespace FlightStorageService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FlightsController : ControllerBase
//Приймає HTTP-запит.
{
    //Контролер працює через інтерфейс, а не через конкретний клас.
    //Це дозволяє легко замінити реалізацію сервісу, наприклад, для тестування
    //або якщо в майбутньому буде інша реалізація.
    private readonly IFlightService _flightService;

    //конструкторна ін'єкція. ASP.NET сам створить FlightService і передасть його в контролер.
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
    //приклад: GET /api/flights/PS123 
    [HttpGet("{flightNumber}")]
    public IActionResult GetByNumber(string flightNumber)
    {
        return Ok($"Searching flight: {flightNumber}");
    }
}