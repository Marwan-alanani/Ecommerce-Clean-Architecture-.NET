using ECommerce_Clean_Arch.Application.Abstractions.Messaging;
using ECommerce_Clean_Arch.Domain.Products;
using SharedKernel.Models;

namespace ECommerce_Clean_Arch.Application.Products.Commands.CreateProduct;

public record CreateProductCommand(
    string Name,
    string Description,
    Money Price,
    string PictureUrl
) : ICommand<Product>;