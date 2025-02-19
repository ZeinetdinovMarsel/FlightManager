using FM.Core.Models;

public class TicketServiceModel
{
    private TicketServiceModel(int id, int serviceId, int ticketId)
    {
        Id = id;
        ServiceId=serviceId;
        TicketId=ticketId;
    }
    public int Id { get; private set; }
    public int ServiceId { get; private set; }
    public int TicketId { get; private set; }

    public static TicketServiceModel Create(int id, int serviceId, int ticketId)
    {
        return new TicketServiceModel(id,serviceId,ticketId);
    }
}
