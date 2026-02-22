namespace ECommerce_Clean_Arch.Contracts.Auth;

public record LoginRequest(
    string Email,
    string Password
);