using Microsoft.AspNetCore.Identity;

namespace ECommerce_Clean_Arch.Domain.Roles;

public sealed class Role : IdentityRole<Guid>, IEquatable<Role>
{
    public bool Equals(Role? other)
    {
        if (GetType() != other?.GetType()) return false;
        return Id.Equals(other.Id);
    }
}