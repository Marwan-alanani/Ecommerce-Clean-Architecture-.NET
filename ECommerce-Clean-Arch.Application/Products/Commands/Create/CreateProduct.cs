using AutoMapper;

using ECommerce_Clean_Arch.Application.Abstractions.Messaging;
using ECommerce_Clean_Arch.Application.Abstractions.Persistence;
using ECommerce_Clean_Arch.Application.Common.Models;
using ECommerce_Clean_Arch.Domain.Categories.ValueObjects;
using ECommerce_Clean_Arch.Domain.Errors.Categories;
using ECommerce_Clean_Arch.Domain.Errors.Products;
using ECommerce_Clean_Arch.Domain.Products;
using ECommerce_Clean_Arch.Domain.Products.ValueObjects;

using Microsoft.EntityFrameworkCore;

using SharedKernel.Errors;
using SharedKernel.Models;
using SharedKernel.Results;

namespace ECommerce_Clean_Arch.Application.Products.Commands.Create;

public record CreateProductCommand(
    string Name,
    string Description,
    MoneyDto Price,
    string PictureUrl,
    CategoryId? CategoryId
) : ICommand<ProductId>;

public class CreateProduct : ICommandHandler<CreateProductCommand, ProductId>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateProduct(IMapper mapper, IApplicationDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<Result<ProductId>> Handle(
        CreateProductCommand request,
        CancellationToken cancellationToken
    )
    {
        if (await _context.Products.AnyAsync(p => p.Name == request.Name, cancellationToken))
        {
            return Error.Conflict(new ProductNameExists(request.Name));
        }


        if (request.CategoryId is not null &&
            !(await _context.Categories.AnyAsync(
                c => c.Id == request.CategoryId.Value,
                cancellationToken))
           )
        {
            return Error.NotFound(new CategoryNotFound(request.CategoryId.Value));
        }

        var price = _mapper.Map<Money>(request.Price);
        var product = Product.Create(
            request.Name,
            request.Description,
            price,
            request.PictureUrl,
            request.CategoryId
        );
        await _context.Products.AddAsync(product, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);


        return product.Id;
    }
}