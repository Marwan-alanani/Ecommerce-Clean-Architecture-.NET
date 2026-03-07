using SharedKernel.Errors;

namespace SharedKernel.Results;

public class Result : IResult
{
    private readonly Error? _error;

    protected Result(Error? error, bool isSuccess)
    {
        _error = error;
        IsSuccess = isSuccess;
    }


    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    public Error Error => (IsFailure ? _error : null)!;

    public static Result Fail(Error error)
    {
        if (error == null) throw new ArgumentNullException(nameof(error));
        return new Result(
            error,
            false);
    }

    public static Result Success()
    {
        return new Result(null, true);
    }

    public static implicit operator Result(Error error)
    {
        return Fail(error);
    }
}

public class Result<T> : Result, IResult<T>
{
    private readonly T? _value;

    private Result(
        T? value,
        Error? error,
        bool isSuccess
    ) : base(error, isSuccess)
    {
        _value = value;
    }

    public T Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException(
            "Cannot access " +
            "value of failure result");

    public new static Result<T> Fail(Error error)
    {
        if (error == null) throw new ArgumentNullException(nameof(error));
        return new Result<T>(
            default,
            error,
            false
        );
    }

    public static Result<T> Success(T value)
    {
        return new Result<T>(
            value,
            null,
            true);
    }

    public static implicit operator Result<T>(Error error) => Fail(error);

    public static implicit operator Result<T>(T value) => Success(value);
}