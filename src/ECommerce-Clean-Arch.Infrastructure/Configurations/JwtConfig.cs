namespace ECommerce_Clean_Arch.Infrastructure.Configurations;

public sealed class JwtConfig
{
    public const string SectionName = "JwtConfig";
    public string Issuer { get; init; } = null!;
    public string Audience { get; init; } = null!;
    public int AccessTokenExpiryInMinutes { get; init; }
    public int RefreshTokenExpiryInDays { get; init; }
    public string SecretKey { get; init; } = null!;
}