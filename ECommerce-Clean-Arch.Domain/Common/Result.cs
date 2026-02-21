using ECommerce_Clean_Arch.Domain.Common.Interfaces;
using ECommerce_Clean_Arch.Domain.Errors.Common;

namespace ECommerce_Clean_Arch.Domain.Common;

public class Result : IResult
{
    private Result(Error? error, bool isSuccess)
    {
        _error = error;
        IsSuccess = isSuccess;
    }

    private readonly Error? _error;


    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    public Error Error => (IsFailure ? _error : null)!;

    public static Result Fail(Error error)
    {
        if (error == null) throw new ArgumentNullException(nameof(error));
        return new(
            error,
            false);
    }

    public static Result Success()
    {
        return new(null, true);
    }

    public static implicit operator Result(Error error)
    {
        return Fail(error);
    }
}

public class Result<T> : IResult<T>
    where T : class
{
    private Result(
        T? value,
        Error? error,
        bool isSuccess
    )
    {
        _error = error;
        _value = value;
        IsSuccess = isSuccess;
    }

    private readonly Error? _error;

    private readonly T? _value;

    public T Value => _value!;
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    public Error Error => (IsFailure ? _error : null)!;


    public static Result<T> Fail(Error error)
    {
        if (error == null) throw new ArgumentNullException(nameof(error));
        return new(
            null,
            error,
            false
        );
    }

    public static Result<T> Success(T value)
    {
        return new(
            value,
            null,
            true);
    }

    public static implicit operator Result<T>(Error error)
    {
        return Fail(error);
    }

    public static implicit operator Result<T>(T value)
    {
        return Success(value);
    }
}