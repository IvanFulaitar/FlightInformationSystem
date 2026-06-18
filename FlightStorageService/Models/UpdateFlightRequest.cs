namespace FlightStorageService.Models;

public class UpdateFlightRequest
{
    public DateTime DepartureDateTime { get; set; }

    public string DepartureAirportCity { get; set; } = string.Empty;

    public string ArrivalAirportCity { get; set; } = string.Empty;

    public int DurationMinutes { get; set; }
}
