using ECommerce_Clean_Arch.Application.Abstractions.Messaging;
using ECommerce_Clean_Arch.Application.Persistence;
using ECommerce_Clean_Arch.Application.Persistence.Repositories;
using ECommerce_Clean_Arch.Domain.RefreshTokens.Enums;

using SharedKernel.Results;

namespace ECommerce_Clean_Arch.Application.Authentication.Commands.LogoutAllSessions;

public sealed record LogoutAllSessionsCommand(string UserId) : ICommand;

public class LogoutAllSessionsCommandHandler : ICommandHandler<LogoutAllSessionsCommand>
{
    private readonly IRefreshTokenRepository _tokenRepository;
    private readonly IUnitOfWork _unitOfWork;

    public LogoutAllSessionsCommandHandler(
        IRefreshTokenRepository tokenRepository,
        IUnitOfWork unitOfWork
    )
    {
        _tokenRepository = tokenRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        LogoutAllSessionsCommand request,
        CancellationToken cancellationToken
    )
    {
        var userIdGuid = Guid.Parse(request.UserId);
        await _tokenRepository.RevokeAllByUserIdAsync(
            userIdGuid,
            RevokedReason.UserLoggedOutAll,
            cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}