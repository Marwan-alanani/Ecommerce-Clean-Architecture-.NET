namespace ECommerce_Clean_Arch.Domain.Users.Constants;

public static class UserRoles
{
    public const string Admin = nameof(Admin);
    public const string User = nameof(User);
    public const string Guest = nameof(Guest);
    public static readonly string[] Roles = [Admin, User, Guest];
}