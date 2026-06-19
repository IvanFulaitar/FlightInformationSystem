namespace FlightClientApp.Models;

public class FlightSearchViewModel
{
    public string? FlightNumber { get; set; }

    public DateTime? Date { get; set; }

    public string? City { get; set; }

    public string? ErrorMessage { get; set; }

    public string? SuccessMessage { get; set; }

    public string? SearchDescription { get; set; }

    public List<FlightViewModel> Results { get; set; } = new();

    public FlightViewModel CreateFlight { get; set; } = new()
    {
        DepartureDateTime = DateTime.Today.AddDays(1).AddHours(10),
        DurationMinutes = 60
    };

    public FlightViewModel UpdateFlight { get; set; } = new()
    {
        DepartureDateTime = DateTime.Today.AddDays(1).AddHours(10),
        DurationMinutes = 60
    };

    public string? DeleteFlightNumber { get; set; }
}
