﻿namespace FM.DataAccess.Entities;
public class ServiceEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Cost { get; set; }
    public ICollection<TicketServiceEntity> TicketService{ get; set; } = new List<TicketServiceEntity>();
}