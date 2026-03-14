namespace ECommerce_Clean_Arch.Domain.Common.Security;

public static partial class Permissions
{
    public static class Users
    {
        public const string Write = "user:write";
        public const string Read = "user:read";
        public const string ViewInActive = "user:viewInActive";

        public static IEnumerable<string> All()
        {
            yield return Write;
            yield return Read;
            yield return ViewInActive;
        }
    }
}