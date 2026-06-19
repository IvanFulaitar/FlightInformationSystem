using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FlightClientApp.Models;
using FlightClientApp.Services;

namespace FlightClientApp.Controllers;

public class HomeController : Controller
{
    private readonly IFlightApiClient _flightApiClient;

    public HomeController(IFlightApiClient flightApiClient)
    {
        _flightApiClient = flightApiClient;
    }

    public IActionResult Index()
    {
        return View(new FlightSearchViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> SearchAll()
    {
        var model = new FlightSearchViewModel
        {
            SearchDescription = "All flights"
        };

        return View("Index", await LoadResultsAsync(model, () => _flightApiClient.GetAllAsync()));
    }

    [HttpPost]
    public async Task<IActionResult> SearchByNumber(FlightSearchViewModel model)
    {
        if (string.IsNullOrWhiteSpace(model.FlightNumber))
        {
            model.ErrorMessage = "Flight number is required.";
            return View("Index", model);
        }

        model.SearchDescription = $"Flight number: {model.FlightNumber.Trim()}";

        try
        {
            var flight = await _flightApiClient.GetByNumberAsync(model.FlightNumber.Trim());
            model.Results = flight == null ? new List<FlightViewModel>() : new List<FlightViewModel> { flight };
        }
        catch (InvalidOperationException exception)
        {
            model.ErrorMessage = exception.Message;
        }

        return View("Index", model);
    }

    [HttpPost]
    public async Task<IActionResult> SearchByDate(FlightSearchViewModel model)
    {
        if (model.Date == null)
        {
            model.ErrorMessage = "Date is required.";
            return View("Index", model);
        }

        model.SearchDescription = $"Departure date: {model.Date:yyyy-MM-dd}";

        return View("Index", await LoadResultsAsync(model, () => _flightApiClient.GetByDateAsync(model.Date.Value)));
    }

    [HttpPost]
    public async Task<IActionResult> SearchByDeparture(FlightSearchViewModel model)
    {
        if (string.IsNullOrWhiteSpace(model.City) || model.Date == null)
        {
            model.ErrorMessage = "Departure city and date are required.";
            return View("Index", model);
        }

        model.SearchDescription = $"Departure from {model.City.Trim()} on {model.Date:yyyy-MM-dd}";

        return View("Index", await LoadResultsAsync(
            model,
            () => _flightApiClient.GetByDepartureCityAndDateAsync(model.City.Trim(), model.Date.Value)));
    }

    [HttpPost]
    public async Task<IActionResult> SearchByArrival(FlightSearchViewModel model)
    {
        if (string.IsNullOrWhiteSpace(model.City) || model.Date == null)
        {
            model.ErrorMessage = "Arrival city and date are required.";
            return View("Index", model);
        }

        model.SearchDescription = $"Arrival to {model.City.Trim()} on {model.Date:yyyy-MM-dd}";

        return View("Index", await LoadResultsAsync(
            model,
            () => _flightApiClient.GetByArrivalCityAndDateAsync(model.City.Trim(), model.Date.Value)));
    }

    [HttpPost]
    public async Task<IActionResult> CreateFlight(FlightSearchViewModel model)
    {
        try
        {
            await _flightApiClient.CreateAsync(model.CreateFlight);
            model.SuccessMessage = $"Flight {model.CreateFlight.FlightNumber} was created.";
            model.SearchDescription = "All flights";
            model.Results = await _flightApiClient.GetAllAsync();
        }
        catch (InvalidOperationException exception)
        {
            model.ErrorMessage = exception.Message;
        }

        return View("Index", model);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateFlight(FlightSearchViewModel model)
    {
        if (string.IsNullOrWhiteSpace(model.UpdateFlight.FlightNumber))
        {
            model.ErrorMessage = "Flight number is required for update.";
            return View("Index", model);
        }

        try
        {
            await _flightApiClient.UpdateAsync(model.UpdateFlight.FlightNumber.Trim(), model.UpdateFlight);
            model.SuccessMessage = $"Flight {model.UpdateFlight.FlightNumber} was updated.";
            model.SearchDescription = "All flights";
            model.Results = await _flightApiClient.GetAllAsync();
        }
        catch (InvalidOperationException exception)
        {
            model.ErrorMessage = exception.Message;
        }

        return View("Index", model);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteFlight(FlightSearchViewModel model)
    {
        if (string.IsNullOrWhiteSpace(model.DeleteFlightNumber))
        {
            model.ErrorMessage = "Flight number is required for delete.";
            return View("Index", model);
        }

        try
        {
            await _flightApiClient.DeleteAsync(model.DeleteFlightNumber.Trim());
            model.SuccessMessage = $"Flight {model.DeleteFlightNumber.Trim()} was deleted.";
            model.SearchDescription = "All flights";
            model.Results = await _flightApiClient.GetAllAsync();
        }
        catch (InvalidOperationException exception)
        {
            model.ErrorMessage = exception.Message;
        }

        return View("Index", model);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    private static async Task<FlightSearchViewModel> LoadResultsAsync(
        FlightSearchViewModel model,
        Func<Task<List<FlightViewModel>>> getResults)
    {
        try
        {
            model.Results = await getResults();
        }
        catch (InvalidOperationException exception)
        {
            model.ErrorMessage = exception.Message;
        }

        return model;
    }
}
