using ECommerce_Clean_Arch.Application.Services;
using ECommerce_Clean_Arch.Domain.Common.Interfaces;
using ECommerce_Clean_Arch.Infrastructure.EventBus;
using ECommerce_Clean_Arch.Infrastructure.Persistence;
using ECommerce_Clean_Arch.Infrastructure.Persistence.Models;

using MediatR;

using Microsoft.EntityFrameworkCore;
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
            var publisher = scope.ServiceProvider.GetRequiredService<IPublisher>();
            var dateTimeProvider = scope.ServiceProvider.GetRequiredService<IDateTimeProvider>();
            var rabbitMqEventBus = scope.ServiceProvider.GetRequiredService<RabbitMqEventBus>();

            // collect messages
            var messages = await applicationDbContext.Set<OutboxMessage>()
                .Where(m => m.ProcessedOn == null)
                .OrderBy(m => m.OccuredOn)
                .Take(BatchSize)
                .ToListAsync(cancellationToken: stoppingToken);

            // publish
            foreach (var message in messages)
            {
                var type = Type.GetType(message.Type);
                if (type == null)
                {
                    // mark processed with failure status or Error
                    message.ProcessedOn = dateTimeProvider.UtcNow;
                    message.Error = $"Type {message.Type} not found";
                    continue;
                }

                var domainEvent = (IDomainEvent?)JsonConvert.DeserializeObject(
                    message.Content,
                    type
                );
                if (domainEvent != null)
                {
                    // TODO: make event handler Idempotent
                    try
                    {
                        await publisher.Publish(domainEvent);
                        await rabbitMqEventBus.PublishAsync(domainEvent);
                    }
                    catch (Exception ex)
                    {
                        message.Error = ex.Message;
                    }

                    message.ProcessedOn = dateTimeProvider.UtcNow;
                }
            }

            await applicationDbContext.SaveChangesAsync(stoppingToken);
            await Task.Delay(10000, stoppingToken);
        }
    }
}