namespace ECommerce_Clean_Arch.Domain.Common.Security;

public static partial class Permissions
{
    public const string ClaimType = "permissions";

    public static IEnumerable<string> GetAll()
    {
        var all = Categories.All();
        all = all.Concat(Orders.All());
        all = all.Concat(Products.All());
        all = all.Concat(Users.All());
        return all;
    }
}