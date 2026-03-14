using ECommerce_Clean_Arch.Domain.RefreshTokens;

using SharedKernel.Errors;

namespace ECommerce_Clean_Arch.Domain.Errors.Token;

public sealed record TokenUserIdNotFound() : ErrorReason(
    ErrorCodes.TokenUserIdNotFound,
    "User id doesn't exist in token"
);