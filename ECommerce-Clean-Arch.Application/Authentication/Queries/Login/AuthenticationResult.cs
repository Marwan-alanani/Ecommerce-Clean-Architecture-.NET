namespace ECommerce_Clean_Arch.Application.Authentication.Queries.Login;

public sealed record RefreshTokenDto(string Token, DateTime Expiration);

public sealed record AuthenticationResult(string AccessToken, RefreshTokenDto RefreshToken);