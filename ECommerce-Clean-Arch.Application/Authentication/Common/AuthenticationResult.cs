using ECommerce_Clean_Arch.Domain.Users;

namespace ECommerce_Clean_Arch.Application.Authentication.Common;

public record AuthenticationResult(
    User User,
    string Token // Jwt access token
);