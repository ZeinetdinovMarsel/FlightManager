using FM.Core.Enums;
using FM.DataAccess.Entities;

public class TicketEntity
{
    public int Id { get; set; }
    public TicketType TicketType { get; set; }  
    public float Price { get; set; } 
    public string Seat { get; set; } = string.Empty; 
    public int FlightId { get; set; }
    public FlightEntity Flight { get; set; }
    public ICollection<TicketServiceEntity> Services { get; set; } = new List<TicketServiceEntity>();
}