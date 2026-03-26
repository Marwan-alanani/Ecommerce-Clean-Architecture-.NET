using SharedKernel.Errors;

namespace ECommerce_Clean_Arch.Domain.Errors.Token;

public sealed record TokenNotFound : ErrorReason
{
    public TokenNotFound(string value) : base(
        ErrorCodes.TokenNotFound,
        $"Passed token value: {value} was not found")
    {
    }
}