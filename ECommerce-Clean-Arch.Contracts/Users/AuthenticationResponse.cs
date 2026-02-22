namespace ECommerce_Clean_Arch.Contracts.Users;

public record AuthenticationResponse(
    Guid Id,
    string Username,
    string Email,
    string FirstName,
    string LastName,
    string Token
);