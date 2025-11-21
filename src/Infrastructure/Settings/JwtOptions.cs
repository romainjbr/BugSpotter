namespace Infrastructure.Settings;

public class JwtOptions
{
    public string? Key { get; set; } 

    public int ExpirationMinutes { get; set; }
}