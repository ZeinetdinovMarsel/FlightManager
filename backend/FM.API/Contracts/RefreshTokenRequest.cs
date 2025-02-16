namespace FM.API.Contracts;
public record RefreshTokenRequest
{
    public string RefreshToken { get; set; } = string.Empty;
}

