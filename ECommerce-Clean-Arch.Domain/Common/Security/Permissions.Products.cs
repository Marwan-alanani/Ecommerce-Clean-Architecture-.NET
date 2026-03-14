namespace ECommerce_Clean_Arch.Domain.Common.Security;

public static partial class Permissions
{
    public static class Products
    {
        public const string Write = "product:write";
        public const string Edit = "product:edit";
        public const string Delete = "product:delete";
        public const string ViewInActive = "product:viewInActive";

        public static IEnumerable<string> All()
        {
            yield return Write;
            yield return Edit;
            yield return Delete;
            yield return ViewInActive;
        }
    }
}