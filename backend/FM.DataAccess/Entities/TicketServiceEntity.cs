using FM.DataAccess.Entities;

public class TicketServiceEntity
{
    public int Id { get; set; }
    public int ServiceId { get; set; }
    public ServiceEntity Service { get; set; }
    public int TicketId { get; set; }
    public TicketEntity Ticket { get; set; }
}