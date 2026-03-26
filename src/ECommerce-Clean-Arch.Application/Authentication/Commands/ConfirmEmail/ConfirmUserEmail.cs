namespace ECommerce_Clean_Arch.Application.Authentication.Commands.ConfirmEmail;

public sealed record ConfirmUserEmailCommand(string Token) : ICommand;

public sealed class ConfirmUserEmailCommandHandler : ICommandHandler<ConfirmUserEmailCommand>
{
    private readonly ICacheService _cacheService;
    private readonly IApplicationDbContext _context;

    public ConfirmUserEmailCommandHandler(
        ICacheService cacheService,
        IApplicationDbContext context
    )
    {
        _cacheService = cacheService;
        _context = context;
    }

    public async Task<Result> Handle(
        ConfirmUserEmailCommand request,
        CancellationToken cancellationToken
    )
    {
        var userId = await _cacheService.GetAsync<Guid?>(request.Token);
        if (userId is null)
        {
            return Error.NotFound(); // TODO:  Add custom error of token not found
        }

        var user = await _context.Users
            .Where(u => u.Id == userId.Value)
            .FirstOrDefaultAsync(cancellationToken);
        if (user is null)
        {
            return Error.NotFound(new UserNotFound(userId.Value));
        }

        user.EmailConfirmed = true;
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}