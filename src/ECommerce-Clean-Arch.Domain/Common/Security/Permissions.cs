namespace ECommerce_Clean_Arch.Domain.Common.Security;

public static partial class Permissions
{
    public const string ClaimType = "permissions";

    public static IEnumerable<string> GetAll()
    {
        return typeof(Permissions)
            .GetNestedTypes()
            .Where(t => t.IsClass)
            .SelectMany(t => t.GetMethod("All")?.Invoke(null, null) as IEnumerable<string> ?? []);
    }
}