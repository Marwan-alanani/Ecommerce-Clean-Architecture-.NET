namespace ECommerce_Clean_Arch.Domain.Users.Constants;

public static class RoleClaims
{
    public const string CreateUser = "user.create";
    public const string CreateProduct = "product.create";
    public static readonly string[] Permissions = [CreateUser, CreateProduct];
}