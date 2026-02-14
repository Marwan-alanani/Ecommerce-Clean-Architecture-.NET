namespace ECommerce_Clean_Arch.Contracts.Users;

public record RegisterUserRequest(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    string Username
);