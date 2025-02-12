namespace FM.DataAccess.Entities;
public class FederalDistrictEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<AirportEntity> Airports { get; set; } = new List<AirportEntity>();

}

