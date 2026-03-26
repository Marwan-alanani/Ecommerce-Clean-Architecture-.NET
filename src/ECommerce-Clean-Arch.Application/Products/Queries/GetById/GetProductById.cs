namespace ECommerce_Clean_Arch.Application.Products.Queries.GetById;

public record GetProductByIdQuery(ProductId Id) : IQuery<ProductDto>;

public class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, ProductDto>
{
    private readonly IApplicationDbContext _context;

    public GetProductByIdQueryHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }

    public async Task<Result<ProductDto>> Handle(
        GetProductByIdQuery request,
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