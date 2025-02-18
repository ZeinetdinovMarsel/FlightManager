namespace FM.API.Contracts;
public record FederalDistrictRequest
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

