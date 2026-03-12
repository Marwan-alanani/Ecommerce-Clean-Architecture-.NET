using ECommerce_Clean_Arch.Domain.RefreshTokens;

using SharedKernel.Errors;

namespace ECommerce_Clean_Arch.Domain.Errors.Token;

public sealed record TokenUserIdNotMatching() : ErrorReason(
    ErrorCodes.TokenNotMatchingUserId,
    "Token doesn't belong to logged in user",
    nameof(RefreshToken.UserId)
);