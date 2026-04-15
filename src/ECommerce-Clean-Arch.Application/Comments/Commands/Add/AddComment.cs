using ECommerce_Clean_Arch.Domain.ProductComments;

namespace ECommerce_Clean_Arch.Application.Comments.Commands.Add;

public sealed record AddCommentCommand(string Content, ProductId ProductId) : ICommand;

public sealed class AddCommentCommandHandler : ICommandHandler<AddCommentCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IUser _user;

    public AddCommentCommandHandler(IApplicationDbContext context, IUser user)
    {
        _context = context;
        _user = user;
    }

    public async Task<Result> Handle(AddCommentCommand request, CancellationToken cancellationToken)
    {
        var productExists = await _context.Products
            .AnyAsync(p => p.Id == request.ProductId, cancellationToken);

        if (_user.Id is null)
        {
            return Error.Security(new UserUnauthenticated());
        }

        if (!productExists)
        {
            return Error.NotFound(new ProductNotFound(request.ProductId));
        }

        var comment = ProductComment.Create(
            _user.UserName!,
            request.ProductId,
            request.Content,
            _user.Id.Value);

        await _context.ProductComments.AddAsync(comment, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}