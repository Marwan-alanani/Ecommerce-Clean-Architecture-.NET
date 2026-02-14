namespace ECommerce_Clean_Arch.Application.Users.Commands.RegisterUser;

public record UserResult(
    Guid Id,
    string Username,
    string Email,
    string FirstName,
    string LastName,
    string Token
);