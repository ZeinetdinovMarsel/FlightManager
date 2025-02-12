namespace FM.API.Contracts;
public record AirportRequest
{
    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public int FederalDistrictId { get; set; }
}

