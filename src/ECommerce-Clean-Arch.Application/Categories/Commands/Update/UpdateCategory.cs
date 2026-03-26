

namespace ECommerce_Clean_Arch.Application.Categories.Commands.Update;

public sealed record UpdateCategoryCommand(
    CategoryId Id,
    string Name
) : ICommand;

public sealed class UpdateCategoryCommandHandler : ICommandHandler<UpdateCategoryCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateCategoryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _context.Categories
            .Where(c => c.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);
        if (category is null)
        {
            return Error.NotFound(new CategoryNotFound(request.Id));
        }

        if (category.Name == request.Name)
        {
            return Result.Success();
        }

        var nameExists = await _context.Categories
            .AnyAsync(c => c.Name == request.Name, cancellationToken);
        if (nameExists)
        {
            return Error.Conflict(new CategoryNameAlreadyExists(request.Name));
        }

        category.Name = request.Name;
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}