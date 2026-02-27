using ECommerce_Clean_Arch.Application.Abstractions.Messaging;

namespace ECommerce_Clean_Arch.Application.Products.Commands.CreateProduct;

public record CreateProductCommand(
    string Name,
    string Description,
    decimal Price,
    string Currency,
    string PictureUrl
) : ICommand<Guid>;