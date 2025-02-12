namespace FM.API.Contracts;
public record FlightRequest
{
    public string FlightNumber { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }
    public int AvailableSeats { get; set; }
    public string AirplanePhotoUrl { get; set; } = string.Empty;
    public int AirportId { get; set; }
}