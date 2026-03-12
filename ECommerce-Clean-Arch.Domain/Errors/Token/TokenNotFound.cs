using ECommerce_Clean_Arch.Domain.RefreshTokens;

using SharedKernel.Errors;

namespace ECommerce_Clean_Arch.Domain.Errors.Token;

public sealed record TokenNotFound(string Value) : ErrorReason(
    ErrorCodes.TokenNotFound,
    $"Passed token value: {Value} was not found",
    nameof(RefreshToken.TokenHash)
);