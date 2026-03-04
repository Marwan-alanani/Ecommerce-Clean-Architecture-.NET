using ECommerce_Clean_Arch.Application.Services;
using ECommerce_Clean_Arch.Domain.Common.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ECommerce_Clean_Arch.Infrastructure.Persistence.Models;
using Newtonsoft.Json;

namespace ECommerce_Clean_Arch.Infrastructure.Persistence.Interceptors;

public class DispatchDomainEventsInterceptor : SaveChangesInterceptor
{
    private readonly IDateTimeProvider _dateTimeProvider;

    public DispatchDomainEventsInterceptor(IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        var dbContext = eventData.Context;
        if (dbContext is null)
            return await base.SavingChangesAsync(
                eventData,
                result,
                cancellationToken);
        await GenerateOutbox(dbContext);
        return await base.SavingChangesAsync(
            eventData,
            result,
            cancellationToken);
    }

    public async Task GenerateOutbox(DbContext dbContext, CancellationToken cancellationToken = default)
    {
        var entities = dbContext.ChangeTracker
            .Entries<IHasDomainEvents>()
            .Select(entry => entry.Entity)
            .Where(entity => entity.DomainEvents.Any())
            .ToList();

        var outboxMessages = entities
            .SelectMany(entity => entity.DomainEvents)
            .Select(ev =>
            {
                var outbox = new OutboxMessage(
                    ev.GetType().ToString(),
                    JsonConvert.SerializeObject(
                        ev,
                        new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.All
                        }
                    ),
                    _dateTimeProvider.UtcNow
                );
                return outbox;
            })
            .ToList();

        entities.ForEach(entity => entity.ClearDomainEvents());


        await dbContext.Set<OutboxMessage>().AddRangeAsync(outboxMessages, cancellationToken);
    }
}