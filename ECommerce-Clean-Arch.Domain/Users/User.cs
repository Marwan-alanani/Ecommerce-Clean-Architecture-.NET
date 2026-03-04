using ECommerce_Clean_Arch.Domain.Common.Interfaces;
using ECommerce_Clean_Arch.Domain.Common.Models;
using ECommerce_Clean_Arch.Domain.Users.Events;
using Microsoft.AspNetCore.Identity;

namespace ECommerce_Clean_Arch.Domain.Users;

public sealed class User : IdentityUser<Guid>, IAuditable, IHasDomainEvents
{
    private List<IDomainEvent> _domainEvents = new();

    private User()
    {
    }

    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }


    public static User Create(
        string userName,
        string firstName,
        string lastName,
        string email
    )
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = userName,
            FirstName = firstName,
            LastName = lastName,
            Email = email
        };
        user.AddDomainEvent(new UserRegistered(email, userName));
        return user;
    }

    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.ToList();

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    public void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}