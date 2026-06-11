namespace FlightStorageService.Models;

//клас який допоможе зберігати інформацію про рейси, які будуть збережені в базі даних
public class Flight
{
    public string FlightNumber { get; set; } = string.Empty;

    public DateTime DepartureDateTime { get; set; }

    public string DepartureAirportCity { get; set; } = string.Empty;

    public string ArrivalAirportCity { get; set; } = string.Empty;

    public int DurationMinutes { get; set; }
}