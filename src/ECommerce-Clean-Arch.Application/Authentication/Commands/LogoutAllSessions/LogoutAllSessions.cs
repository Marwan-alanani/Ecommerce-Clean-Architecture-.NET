namespace ECommerce_Clean_Arch.Application.Authentication.Commands.LogoutAllSessions;

public sealed record LogoutAllSessionsCommand : ICommand;

public class LogoutAllSessionsCommandHandler : ICommandHandler<LogoutAllSessionsCommand>
{
    private readonly ISessionRepository _tokenRepository;
    private readonly IApplicationDbContext _context;
    private readonly IUser _user;
    private readonly ICookieService _cookieService;

    public LogoutAllSessionsCommandHandler(
        ISessionRepository tokenRepository,
        IUser user,
        IApplicationDbContext context,
        ICookieService cookieService
    )
    {
        _tokenRepository = tokenRepository;
        _user = user;
        _context = context;
        _cookieService = cookieService;
    }

    public async Task<Result> Handle(
        LogoutAllSessionsCommand request,
        CancellationToken cancellationToken
    )
    {
        if (_user.Id is null)
        {
            var error = Error.NotFound(new TokenUserIdNotFound());
            return error;
        }

        await _tokenRepository.RevokeAllByUserIdAsync(
            _user.Id.Value,
            RevokedReason.LoggedOutAll,
            cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        _cookieService.ClearRefreshToken();
        return Result.Success();
    }
}