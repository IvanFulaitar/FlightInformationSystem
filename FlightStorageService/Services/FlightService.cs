using FlightStorageService.Models;
using FlightStorageService.Repositories;

namespace FlightStorageService.Services;

// Сервісний шар.
//
// Містить бізнес-логіку застосунку.
// Controller не працює напряму з Repository,
// а звертається до Service.
//
// Питання сервісу:
// "Що потрібно зробити?"

public class FlightService : IFlightService
{
    // Доступ до джерела даних через Repository.
    private readonly IFlightRepository _flightRepository;

    // Dependency Injection.
    // ASP.NET автоматично створює Repository
    // та передає його в сервіс.
    public FlightService(IFlightRepository flightRepository)
    {
        _flightRepository = flightRepository;
    }

    // Тестовий метод.
    // Поки що просто повертає тестовий рейс з Repository.
    public Flight GetTestFlight()
    {
        return _flightRepository.GetTestFlight();
    }

    // Бізнес-логіка пошуку рейсу за номером.
    //
    // Тут можна виконувати:
    // - валідацію даних
    // - перевірку правил
    // - додаткову обробку
    //
    // Якщо номер рейсу не передано,
    // повертаємо null.
    public Flight? GetByNumber(string flightNumber)
    {
        if (string.IsNullOrWhiteSpace(flightNumber))
        {
            return null;
        }
        // Поки що повертаємо тестовий рейс.
        // Пізніше тут буде пошук через Repository у SQL Server.
        return _flightRepository.GetTestFlight();
    }

    public List<Flight> GetByDate(DateTime date)
    {
        if (date.Date < DateTime.Today || date.Date > DateTime.Today.AddDays(7))
        {
            return [];
        }
        return _flightRepository.GetTestFlight();
    }

    public List<Flight> GetByDepartureCityAndDate(string city, DateTime date)
    {
        if (string.IsNullOrWhiteSpace(city))
        {
            return [];
        }

        if (date.Date < DateTime.Today || date.Date > DateTime.Today.AddDays(7))
        {
            return [];
        }

        return _flightRepository.GetByDepartureCityAndDate(city, date);
    }

    public List<Flight> GetByArrivalCityAndDate(string city, DateTime date)
    {
        if (string.IsNullOrWhiteSpace(city))
        {
            return [];
        }

        if (date.Date < DateTime.Today || date.Date > DateTime.Today.AddDays(7))
        {
            return [];
        }

        return _flightRepository.GetByArrivalCityAndDate(city, date);
    }
}