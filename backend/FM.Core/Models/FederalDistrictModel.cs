namespace FM.Core.Models;
public class FederalDistrictModel
{
    private FederalDistrictModel(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public static FederalDistrictModel Create(int id, string name)
    {
        return new FederalDistrictModel(id, name);
    }
}

