namespace ECommerce_Clean_Arch.Domain.Common.Security;

public static partial class Permissions
{
    public static class Orders
    {
        public const string ViewAll = "orders:viewAll";

        public static IEnumerable<string> All()
        {
            yield return ViewAll;
        }
    }
}