namespace FM.Core.Models;
public class AirportModel
{
    private AirportModel(int id, string name, string city, int federalDistrictId)
    {
        Id = id;
        Name = name;
        City = city;
        FederalDistrictId = federalDistrictId;
    }

    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string City { get; private set; } = string.Empty;
    public int FederalDistrictId { get; private set; }

    public static AirportModel Create(int id, string name, string city, int federalDistrictId)
    {
        return new AirportModel(id, name, city, federalDistrictId);
    }
}

