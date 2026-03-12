using SharedKernel.Errors;

namespace ECommerce_Clean_Arch.Domain.Errors.Token;

public sealed record MissingTokenCookie() : ErrorReason(
    ErrorCodes.MissingTokenCookie,
    "Token not found in cookies",
    "HTTP only cookies"
);