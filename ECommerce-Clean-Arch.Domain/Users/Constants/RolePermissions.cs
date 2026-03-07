namespace ECommerce_Clean_Arch.Domain.Users.Constants;

public static class RolePermissions
{
    public const string CreateUser = "user.create";
    public const string CreateProduct = "product.create";
    public static readonly string[] Permissions = [CreateUser, CreateProduct];
    public const string ClaimType = "Permission";
}