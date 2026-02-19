using ECommerce_Clean_Arch.Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace ECommerce_Clean_Arch.Domain.Users;

public sealed class User : IdentityUser<Guid>, IAuditable
{
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    private User()
    {
    }


    public static User Create(
        string userName,
        string firstName,
        string lastName,
        string email
    )
    {
        return new User()
        {
            Id = Guid.NewGuid(),
            UserName = userName,
            FirstName = firstName,
            LastName = lastName,
            Email = email,
        };
    }

}