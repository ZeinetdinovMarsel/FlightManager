namespace FM.API.Contracts;

public record ServiceRequest
{
    public string Name { get; set; } = string.Empty;
    public decimal Cost { get; set; }
}