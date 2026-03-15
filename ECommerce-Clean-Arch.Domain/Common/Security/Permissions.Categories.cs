namespace ECommerce_Clean_Arch.Domain.Common.Security;

public static partial class Permissions
{
    public static class Categories
    {
        public const string Create = "category:create";
        public const string Update = "category:update";

        public static IEnumerable<string> All()
        {
            yield return Create;
            yield return Update;
        }
    }
}