using ECommerce_Clean_Arch.Domain.Common.Models;
using ECommerce_Clean_Arch.Domain.Users;
using ECommerce_Clean_Arch.Infrastructure.Persistence.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ECommerce_Clean_Arch.Infrastructure.Persistence;

public class IdentityDbContext(DbContextOptions<IdentityDbContext> options) :
    IdentityDbContext<User, IdentityRole<Guid>, Guid>(options)
{
    public const string ConnectionStringName = "IdentityDb";

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new UserConfigurations())
            .ApplyConfiguration(new OutboxMessageConfigurations())
            .Ignore<IDomainEvent>();
        base.OnModelCreating(builder);
    }
}