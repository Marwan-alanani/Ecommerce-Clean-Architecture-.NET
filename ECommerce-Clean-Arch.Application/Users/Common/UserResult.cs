using ECommerce_Clean_Arch.Domain.Users;

namespace ECommerce_Clean_Arch.Application.Users.Common;

public record UserResult(
    User User,
    string Token // Jwt access token
);