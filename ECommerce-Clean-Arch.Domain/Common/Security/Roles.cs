namespace ECommerce_Clean_Arch.Domain.Common.Security;

public static class Roles
{
    public const string Admin = nameof(Admin);
    public const string User = nameof(User);
    public const string Guest = nameof(Guest);

    public static IEnumerable<string> GetAll()
    {
        yield return Admin;
        yield return User;
        yield return Guest;
    }
}