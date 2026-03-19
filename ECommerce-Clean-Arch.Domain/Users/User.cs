using ECommerce_Clean_Arch.Domain.Common.Interfaces;
using ECommerce_Clean_Arch.Domain.Users.Events;

using Microsoft.AspNetCore.Identity;

namespace ECommerce_Clean_Arch.Domain.Users;

public sealed class User : IdentityUser<Guid>, IAuditable, IEquatable<User>
{
    private readonly List<IDomainEvent> _domainEvents = new();

#pragma warning disable CS8618
    private User()
    {
    }
#pragma warning restore CS8618

    // ReSharper disable once CollectionNeverUpdated.Local
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public Guid? CreatedBy { get; set; }
    public Guid? LastModifiedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastModifiedAt { get; set; }
    public long Version { get; set; }
    public bool IsActive { get; private set; }

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
            Email = email,
            IsActive = true
        };
        user.AddDomainEvent(
            new UserRegisteredEvent(
                email,
                userName)
        );
        return user;
    }

    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.ToList();

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    public void AddDomainEvent(IDomainEvent @event)
    {
        @event.AggregateId = Id;
        @event.AggregateVersion = Version;

        _domainEvents.Add(@event);
        Version++;
    }

    public bool Equals(User? other)
    {
        if (GetType() != other?.GetType()) return false;
        return Id.Equals(other.Id);
    }

    public void Deactivate()
    {
        AddDomainEvent(new UserDeactivatedEvent());
        IsActive = false;
    }

    public void Activate()
    {
        IsActive = false;
    }
}