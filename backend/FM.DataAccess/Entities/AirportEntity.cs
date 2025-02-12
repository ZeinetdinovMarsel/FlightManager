using FM.DataAccess.Entities;

public class AirportEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public int FederalDistrictId { get; set; } 
    public FederalDistrictEntity FederalDistrict { get; set; }
    public ICollection<FlightEntity> Flights { get; set; } = new List<FlightEntity>();
}