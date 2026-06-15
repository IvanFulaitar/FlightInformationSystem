using Microsoft.AspNetCore.Mvc;
using FlightStorageService.Services;
using FlightStorageService.Models;

namespace FlightStorageService.Controllers;

[ApiController]
[Route("api/[controller]")]
// Контролер є точкою входу для HTTP-запитів.
// Приймає запит від клієнта, викликає сервіс та повертає відповідь.
public class FlightsController : ControllerBase
{
    private readonly IFlightService _flightService;


    // Контролер працює з бізнес-логікою через сервісний шар.
    public FlightsController(IFlightService flightService)
    {
        _flightService = flightService;
    }

    // Отримати рейс за номером.
    //
    // Приклад:
    // GET /api/flights/PS123
    //
    // Повертає:
    // 200 OK - якщо рейс знайдено
    // 404 Not Found - якщо рейс відсутній
    // 400 Bad Request - якщо номер рейсу некоректний
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

    // Отримати всі рейси на конкретну дату.
    //
    // Приклад:
    // GET /api/flights?date=2026-06-15
    //
    // Повертає список рейсів або 404, якщо рейсів на цю дату немає.
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

    // Пошук рейсів за містом відправлення та датою.
    //
    // Приклад:
    // GET /api/flights/departure?city=Kyiv&date=2026-06-15
    //
    // Повертає список знайдених рейсів або 404, якщо збігів немає.
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
    // Пошук рейсів за містом прибуття та датою.
    //
    // Приклад:
    // GET /api/flights/arrival?city=London&date=2026-06-15
    //
    // Повертає список знайдених рейсів або 404, якщо збігів немає.
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

    // Створення нового рейсу.
    //
    // Дані рейсу передаються у тілі HTTP-запиту.
    //
    // Повертає:
    // 201 Created - якщо рейс успішно створено
    // 400 Bad Request - якщо дані не пройшли валідацію
    [HttpPost]
    public IActionResult AddFlight([FromBody] Flight flight)
    {
        try
        {
            _flightService.AddFlight(flight);

            return CreatedAtAction(
                nameof(GetByNumber),
                new { flightNumber = flight.FlightNumber },
                flight);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(exception.Message);
        }
    }


    // Отримати всі рейси.
    [HttpGet("all")]
    public IActionResult GetAll()
    {
        var flights = _flightService.GetAll();

        if (flights.Count == 0)
        {
            return NotFound();
        }

        return Ok(flights);
    }

    // Оновити існуючий рейс за номером.
    [HttpPut("{flightNumber}")]
    public IActionResult UpdateFlight(string flightNumber, [FromBody] Flight flight)
    {
        try
        {
            _flightService.UpdateFlight(flightNumber, flight);
            return NoContent();
        }
        catch (ArgumentException exception)
        {
            return BadRequest(exception.Message);
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(exception.Message);
        }
    }

    // Видалити рейс за номером.
    [HttpDelete("{flightNumber}")]
    public IActionResult DeleteFlight(string flightNumber)
    {
        try
        {
            _flightService.DeleteFlight(flightNumber);
            return NoContent();
        }
        catch (ArgumentException exception)
        {
            return BadRequest(exception.Message);
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(exception.Message);
        }
    }
}
