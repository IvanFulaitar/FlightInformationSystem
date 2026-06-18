namespace FlightStorageService.Exceptions;

public class DuplicateFlightException : Exception
{
    public DuplicateFlightException(string message)
        : base(message)
    {
    }
}
