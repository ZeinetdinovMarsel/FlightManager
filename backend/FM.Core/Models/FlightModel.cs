namespace FM.Core.Models;
public class FlightModel
{
    private FlightModel(int id, string flightNumber, string destination, DateTime departureTime, DateTime arrivalTime, int availableSeats, string airplanePhotoUrl, int airportId)
    {
        Id = id;
        FlightNumber = flightNumber;
        Destination = destination;
        DepartureTime = departureTime;
        ArrivalTime = arrivalTime;
        AvailableSeats = availableSeats;
        AirplanePhotoUrl = airplanePhotoUrl;
        AirportId = airportId;
    }

    public int Id { get; private set; }
    public string FlightNumber { get; private set; } = string.Empty;
    public string Destination { get; private set; } = string.Empty;
    public DateTime DepartureTime { get; private set; }
    public DateTime ArrivalTime { get; private set; }
    public int AvailableSeats { get; private set; }
    public string AirplanePhotoUrl { get; private set; } = string.Empty;
    public int AirportId { get; private set; }

    public static FlightModel Create(int id, string flightNumber, string destination, DateTime departureTime, DateTime arrivalTime, int availableSeats, string airplanePhotoUrl, int airportId)
    {
        return new FlightModel(id, flightNumber, destination, departureTime, arrivalTime, availableSeats, airplanePhotoUrl, airportId);
    }
}