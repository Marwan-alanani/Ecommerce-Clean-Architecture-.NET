using ECommerce_Clean_Arch.Application.Abstractions.Messaging;
using ECommerce_Clean_Arch.Application.Abstractions.Persistence;
using ECommerce_Clean_Arch.Application.Abstractions.Persistence.Repositories;
using ECommerce_Clean_Arch.Application.Authentication.Services;
using ECommerce_Clean_Arch.Application.Services;
using ECommerce_Clean_Arch.Domain.UserSessions.Enums;

using Microsoft.Extensions.Logging;

using SharedKernel.Results;

namespace ECommerce_Clean_Arch.Application.Authentication.Commands.Logout;

public sealed record LogoutCommand : ICommand;

public sealed class LogoutCommandHandler : ICommandHandler<LogoutCommand>
{
    private readonly ISessionRepository _sessionRepository;
    private readonly IApplicationDbContext _context;
    private readonly ICookieService _cookieService;
    private readonly ILogger<LogoutCommandHandler> _logger;

    public LogoutCommandHandler(
        ISessionRepository sessionRepository,
        IApplicationDbContext context,
        ILogger<LogoutCommandHandler> logger,
        ICookieService cookieService
    )
    {
        _sessionRepository = sessionRepository;
        _context = context;
        _logger = logger;
        _cookieService = cookieService;
    }

    public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        // 1. Get token
        var tokenValue = _cookieService.GetRefreshToken();
        if (tokenValue == null)
        {
            return Result.Success();
        }

        // 2. Revoke token
        await _sessionRepository.RevokeByValueAsync(
            tokenValue,
            RevokedReason.LoggedOut,
            cancellationToken
        );
        // 3. Remove cookie
        _cookieService.ClearRefreshToken();
        // 4. Save changes
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}