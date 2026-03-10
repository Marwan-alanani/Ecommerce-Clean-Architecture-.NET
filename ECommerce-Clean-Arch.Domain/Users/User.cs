using ECommerce_Clean_Arch.Domain.Common.Interfaces;
using ECommerce_Clean_Arch.Domain.Users.Entities;
using ECommerce_Clean_Arch.Domain.Users.Events;

using Microsoft.AspNetCore.Identity;

namespace ECommerce_Clean_Arch.Domain.Users;

public sealed class User : IdentityUser<Guid>, IAuditable, IAggregateRoot
{
    private readonly List<IDomainEvent> _domainEvents = new();

#pragma warning disable CS8618
    private User()
    {
    }
#pragma warning restore CS8618

    // ReSharper disable once CollectionNeverUpdated.Local
    private readonly List<RefreshToken> _refreshTokens = new();
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public Guid? CreatedBy { get; set; }
    public Guid? LastModifiedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastModifiedAt { get; set; }
    public long Version { get; set; }
    public IReadOnlyList<RefreshToken> RefreshTokens => _refreshTokens.ToList();

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
        user.AddDomainEvent(
            new UserRegistered(
                user.Id,
                email,
                userName,
                user.Version)
        );
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
        Version++;
    }

    public void AddRefreshToken(RefreshToken token)
    {
        _refreshTokens.Add(token);
    }
}