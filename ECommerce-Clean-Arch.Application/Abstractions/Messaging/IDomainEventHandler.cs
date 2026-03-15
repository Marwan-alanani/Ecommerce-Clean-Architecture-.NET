using ECommerce_Clean_Arch.Domain.Common.Interfaces;
using MediatR;

namespace ECommerce_Clean_Arch.Application.Abstractions.Messaging;

public interface IDomainEventHandler<in T> : INotificationHandler<T>
    where T : IDomainEvent;