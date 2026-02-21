using ECommerce_Clean_Arch.Domain.Errors.Common;

namespace ECommerce_Clean_Arch.Domain.Common.Interfaces;

public interface IResult
{
    bool IsSuccess { get; }
    bool IsFailure { get; }
    Error Error { get; }
}

public interface IResult<T> : IResult
{
    T Value { get; }
}