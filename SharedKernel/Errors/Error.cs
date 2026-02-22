namespace SharedKernel.Errors;

public sealed record Error // One error can have 1...N reasons
{
    private readonly List<ErrorReason> _reasons = new();

    private Error(
        string code,
        string message,
        string description,
        ErrorType type,
        params ErrorReason[] reasons
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
    public IReadOnlyList<ErrorReason> Reasons => _reasons.AsReadOnly();
    public ErrorType Type { get; }

    public static Error Validation(params ErrorReason[] reasons)
    {
        return new Error(
            "ValidationError",
            "A validation error occured",
            "One or more validation errors occured",
            ErrorType.Validation,
            reasons
        );
    }

    public static Error Conflict(params ErrorReason[] reasons)
    {
        return new Error(
            "Conflict",
            "A conflict occured",
            "There has been a conflict ... cannot continue operation",
            ErrorType.Conflict,
            reasons
        );
    }

    public static Error NotFound(params ErrorReason[] reasons)
    {
        return new Error(
            "NotFound",
            "Resource not found",
            "Cannot allocate given resource",
            ErrorType.NotFound,
            reasons
        );
    }

    public void AddReason(ErrorReason errorReason)
    {
        _reasons.Add(errorReason);
    }

    public void AddReason(
        string code,
        string description,
        string? field = null
    )
    {
        _reasons.Add(
            new ErrorReason(
                code,
                description,
                field)
        );
    }
}

public record ErrorReason(
    string Code,
    string Description,
    string? Field = null
);