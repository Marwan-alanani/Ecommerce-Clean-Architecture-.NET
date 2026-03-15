using ECommerce_Clean_Arch.Application.Abstractions.Messaging;
using ECommerce_Clean_Arch.Application.Persistence.Repositories;
using ECommerce_Clean_Arch.Domain.Categories.Events;
using ECommerce_Clean_Arch.Domain.Categories.ValueObjects;

namespace ECommerce_Clean_Arch.Application.Categories.EventHandlers;

public sealed class CategoryDeactivatedEventHandler : IDomainEventHandler<CategoryDeactivatedEvent>
{
    private readonly IProductRepository _productRepository;

    public CategoryDeactivatedEventHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task Handle(CategoryDeactivatedEvent notification, CancellationToken cancellationToken)
    {
        var categoryId = CategoryId.FromValue(notification.AggregateId);
        await _productRepository.DeactivateByCategoryIdAsync(categoryId, cancellationToken);
    }
}