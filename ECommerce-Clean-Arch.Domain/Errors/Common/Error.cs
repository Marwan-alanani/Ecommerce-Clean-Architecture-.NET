namespace ECommerce_Clean_Arch.Domain.Errors.Common;

public sealed record Error // One error can have 1...N reasons
{
    private readonly List<IReason> _reasons = new();

    private Error(
        string code,
        string message,
        string description,
        ErrorType type,
        params IReason[] reasons
    )
    {
        Message = message;
        Code = code;
        Description = description;
        Type = type;
        _reasons.AddRange(reasons);
    }

    public string Code { get; }
    public string Message { get; }
    public string Description { get; }
    public IReadOnlyList<IReason> Reasons => _reasons.AsReadOnly();
    public ErrorType Type { get; }

    public static Error Validation(params IReason[] reasons)
    {
        return new(
            "ValidationError",
            "A validation error occured",
            "One or more validation errors occured",
            ErrorType.Validation,
            reasons
        );
    }

    public static Error Conflict(params IReason[] reasons)
    {
        return new(
            "Conflict",
            "A conflict occured",
            "There has been a conflict ... cannot continue operation",
            ErrorType.Conflict,
            reasons
        );
    }

    public static Error NotFound(params IReason[] reasons)
    {
        return new(
            "NotFound",
            "Resource not found",
            "Cannot allocate given resource",
            ErrorType.NotFound,
            reasons
        );
    }

    public void AddReason(IReason reason)
    {
        _reasons.Add(reason);
    }

    public void AddReason(
        string code,
        string description,
        string? field = null
    )
    {
        _reasons.Add(
            new Reason(
                code,
                description,
                field)
        );
    }
}

public interface IReason
{
    public string Code { get; }
    public string Description { get; }
    public string? Field { get; }
};

public record Reason(
    string Code,
    string Description,
    string? Field = null
) : IReason;