namespace ECommerce_Clean_Arch.Application.Products.Commands.Update;

public record UpdateProductCommand(
    ProductId Id,
    string? Name,
    string? Description,
    MoneyFlat? Price,
    CategoryId? CategoryId
) : ICommand
{
    private class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<UpdateProductCommand, Product>()
                .ForAllMembers(opt => opt.Condition((
                    _,
                    _,
                    srcMember
                ) => srcMember is not null));
        }
    }
}

public class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand>
{
    private readonly IMapper _mapper;
    private readonly IApplicationDbContext _context;

    public UpdateProductCommandHandler(
        IMapper mapper,
        IApplicationDbContext context
    )
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<Result> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _context.Products
            .Where(p => p.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);
        if (product is null)
            return Error.NotFound(new ProductNotFound(request.Id));
        var productNameExists = await _context.Products
            .AnyAsync(p => p.Name == request.Name, cancellationToken);
        if (request.Name is not null && product.Name != request.Name && productNameExists)
        {
            return Error.Validation(new ProductNameExists(request.Name));
        }

        var categoryExistsAndActive = await _context.Categories
            .Where(c => c.Id == request.CategoryId)
            .Where(c => c.IsActive)
            .AnyAsync(cancellationToken);

        if (request.CategoryId is not null && !categoryExistsAndActive)
        {
            return Error.Validation(new CategoryNotFound(request.CategoryId.Value));
        }

        _mapper.Map(request, product);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}