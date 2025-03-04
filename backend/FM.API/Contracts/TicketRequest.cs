﻿using FM.Core.Enums;

public record TicketRequest
{
    public TicketType TicketType { get; set; }
    public float Price { get; set; }
    public string Seat { get; set; } = string.Empty;
    public int FlightId { get; set; }
    public List<TicketServiceRequest> Services { get; set; } = new List<TicketServiceRequest>();
}

public record TicketServiceRequest
{
    public int ServiceId { get; set; }
}
