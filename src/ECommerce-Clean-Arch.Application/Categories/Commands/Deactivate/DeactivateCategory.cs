
namespace ECommerce_Clean_Arch.Application.Categories.Commands.Deactivate;

public sealed record DeactivateCategoryCommand(CategoryId Id) : ICommand;

public sealed class DeactivateCategoryCommandHandler : ICommandHandler<DeactivateCategoryCommand>
{
    private readonly IApplicationDbContext _context;

    public DeactivateCategoryCommandHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }

    public async Task<Result> Handle(
        DeactivateCategoryCommand request,
        CancellationToken cancellationToken
    )
    {
        var category = await _context.Categories
            .Where(c => c.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);
        if (category is null)
        {
            return Error.NotFound(new CategoryNotFound(request.Id));
        }

        category.Deactivate();
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}