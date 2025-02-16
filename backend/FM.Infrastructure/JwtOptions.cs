namespace FM.Infrastructure;
public class JwtOptions
{
    public string SecretKey { get; set; } = string.Empty;
    public int ActivationTokenExpiredMinutes { get; set; }
    public int RefreshTokenExpiredHours { get; set; }
}
