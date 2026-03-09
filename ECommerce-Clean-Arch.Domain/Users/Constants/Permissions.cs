namespace ECommerce_Clean_Arch.Domain.Users.Constants;

public abstract class Permissions
{
    public const string UserWrite = "user:write";
    public const string UserRead = "user:read";
    public const string ProductWrite = "product:write";
    public const string ProductEdit = "product:edit";
    public const string ProductDelete = "product:delete";
    public const string ViewActive = "view:active";


    public static readonly string[] AllPermissions =
        [UserWrite, ProductWrite, ViewActive, UserRead, ProductEdit, ProductDelete];

    public const string ClaimType = "permissions";
}