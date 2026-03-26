namespace SharedKernel.Errors;

// make this a class
public record ErrorReason(
    string Code,
    string Description,
    string? Field = null
)
{
    public ErrorReason(Exception exception) : this(
        exception.GetType().Name,
        exception.Message)
    {
    }
}