using FM.Core.Enums;

public class TicketModel
{
    private TicketModel(int id, TicketType ticketType, float price, string seat, int flightId, ICollection<TicketServiceModel> services)
    {
        Id = id;
        TicketType = ticketType;
        Price = price;
        Seat = seat;
        FlightId = flightId;
        Services = services;
    }

    public int Id { get; private set; }
    public TicketType TicketType { get; private set; }
    public float Price { get; private set; }
    public string Seat { get; private set; } = string.Empty;
    public int FlightId { get; private set; }
    public ICollection<TicketServiceModel> Services { get; private set; } = new List<TicketServiceModel>();
    public static TicketModel Create(int id, TicketType ticketType, float price, string seat, int flightId, ICollection<TicketServiceModel> services)
    {
        return new TicketModel(id, ticketType, price, seat, flightId, services);
    }
}
