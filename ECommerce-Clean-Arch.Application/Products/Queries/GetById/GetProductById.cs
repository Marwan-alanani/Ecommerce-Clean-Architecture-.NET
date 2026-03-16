using ECommerce_Clean_Arch.Application.Abstractions.Messaging;
using ECommerce_Clean_Arch.Application.Abstractions.Persistence;
using ECommerce_Clean_Arch.Application.Products.Common;
using ECommerce_Clean_Arch.Domain.Errors.Products;
using ECommerce_Clean_Arch.Domain.Products.ValueObjects;

using Microsoft.EntityFrameworkCore;

using SharedKernel.Errors;
using SharedKernel.Results;

namespace ECommerce_Clean_Arch.Application.Products.Queries.GetById;

public record GetProductById(ProductId Id) : IQuery<ProductDto>;

public class GetProductByIdQueryHandler : IQueryHandler<GetProductById, ProductDto>
{
    private readonly IApplicationDbContext _context;

    public GetProductByIdQueryHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }

    public async Task<Result<ProductDto>> Handle(
        GetProductById request,
        CancellationToken cancellationToken
    )
    {
        var productDto = await _context.Products.AsNoTracking()
            .Where(p => p.Id == request.Id)
            .ToProductDto(_context)
            .FirstOrDefaultAsync(cancellationToken);
        if (productDto is null)
        {
            return Error.NotFound(new ProductNotFound(request.Id));
        }

        return productDto;
    }
}