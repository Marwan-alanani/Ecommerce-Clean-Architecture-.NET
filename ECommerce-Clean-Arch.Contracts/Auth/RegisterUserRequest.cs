namespace ECommerce_Clean_Arch.Contracts.Auth;

public record RegisterUserRequest(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    string Username
);