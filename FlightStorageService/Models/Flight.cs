namespace FlightStorageService.Models;

// Модель рейсу, яка зберігається у базі даних.
public class Flight
{
    public string FlightNumber { get; set; } = string.Empty;

    public DateTime DepartureDateTime { get; set; }

    public string DepartureAirportCity { get; set; } = string.Empty;

    public string ArrivalAirportCity { get; set; } = string.Empty;

    public int DurationMinutes { get; set; }
}
