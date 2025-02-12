namespace FM.DataAccess.Entities;
public class FlightEntity
{
    public int Id { get; set; }
    public string FlightNumber { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }
    public int AvailableSeats { get; set; }
    public string AirplanePhotoUrl { get; set; } = string.Empty;

    public int AirportId { get; set; }
    public AirportEntity Airport { get; set; }

    public ICollection<TicketEntity> Tickets { get; set; } = new List<TicketEntity>();
}
