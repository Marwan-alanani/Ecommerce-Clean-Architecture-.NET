namespace ECommerce_Clean_Arch.Infrastructure.Authentication;

public class JwtConfig
{
    public const string SectionName = "JwtConfig";
    public string Issuer { get; init; } = null!;
    public string Audience { get; init; } = null!;
    public int ExpiryInMinutes { get; init; }
    public string SecretKey { get; init; } = null!;
}