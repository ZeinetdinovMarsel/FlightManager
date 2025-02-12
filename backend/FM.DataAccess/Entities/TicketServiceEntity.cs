using FM.DataAccess.Entities;

public class TicketServiceEntity
{
    public int Id { get; set; }
    public string ServiceName { get; set; } = string.Empty;
    public float ServiceCost { get; set; }
    public int TicketId { get; set; }
    public TicketEntity Ticket { get; set; }
}