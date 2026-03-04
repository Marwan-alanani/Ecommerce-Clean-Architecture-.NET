using ECommerce_Clean_Arch.Application.Services;
using ECommerce_Clean_Arch.Domain.Common.Models;
using ECommerce_Clean_Arch.Infrastructure.Persistence;
using ECommerce_Clean_Arch.Infrastructure.Persistence.Models;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace ECommerce_Clean_Arch.Infrastructure.BackgroundServices;

public class PublishOutboxMessages : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    const int BatchSize = 20;

    public PublishOutboxMessages(
        IServiceScopeFactory scopeFactory
    )
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();
            var applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var identityDbContext = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
            var publisher = scope.ServiceProvider.GetRequiredService<IPublisher>();
            var dateTimeProvider = scope.ServiceProvider.GetRequiredService<IDateTimeProvider>();

            // take messages
            var messages = identityDbContext.Set<OutboxMessage>()
                .Where(m => m.ProcessedOn == null)
                .Take(BatchSize)
                .ToList();

            messages.AddRange(
                applicationDbContext.Set<OutboxMessage>()
                    .Where(m => m.ProcessedOn == null)
                    .Take(BatchSize)
                    .ToList());

            // publish
            foreach (var message in messages)
            {
                var domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(
                    message.Content,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    });
                if (domainEvent != null)
                    await publisher.Publish(domainEvent);
            }

            // mark processed
            messages.ForEach(m => m.ProcessedOn = dateTimeProvider.UtcNow);
            await identityDbContext.SaveChangesAsync();
            await Task.Delay(10000, stoppingToken);
        }
    }
}