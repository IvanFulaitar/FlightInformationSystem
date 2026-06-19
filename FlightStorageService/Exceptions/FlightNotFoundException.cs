namespace FlightStorageService.Exceptions;

public class FlightNotFoundException : Exception
{
    public FlightNotFoundException(string message)
        : base(message)
    {
    }
}
