namespace ECommerce_Clean_Arch.Application.Products.Commands.Create;

public record CreateProductCommand(
    string Name,
    string Description,
    MoneyFlat Price,
    string PictureUrl,
    CategoryId CategoryId
) : ICommand<ProductId>;

public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, ProductId>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateProductCommandHandler(IMapper mapper, IApplicationDbContext context)
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


        if (!(await _context.Categories
                .Where(c => c.Id == request.CategoryId)
                .Where(c => c.IsActive)
                .AnyAsync(cancellationToken))
           )
        {
            return Error.NotFound(new CategoryNotFound(request.CategoryId));
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