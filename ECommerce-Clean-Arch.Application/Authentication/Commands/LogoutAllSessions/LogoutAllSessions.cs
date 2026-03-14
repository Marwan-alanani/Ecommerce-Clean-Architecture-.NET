using ECommerce_Clean_Arch.Application.Abstractions.Messaging;
using ECommerce_Clean_Arch.Application.Common.Interfaces;
using ECommerce_Clean_Arch.Application.Persistence;
using ECommerce_Clean_Arch.Application.Persistence.Repositories;
using ECommerce_Clean_Arch.Domain.Errors.Token;
using ECommerce_Clean_Arch.Domain.RefreshTokens.Enums;

using SharedKernel.Errors;
using SharedKernel.Results;

namespace ECommerce_Clean_Arch.Application.Authentication.Commands.LogoutAllSessions;

public sealed record LogoutAllSessionsCommand : ICommand;

public class LogoutAllSessionsCommandHandler : ICommandHandler<LogoutAllSessionsCommand>
{
    private readonly IRefreshTokenRepository _tokenRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUser _user;

    public LogoutAllSessionsCommandHandler(
        IRefreshTokenRepository tokenRepository,
        IUnitOfWork unitOfWork,
        IUser user
    )
    {
        _tokenRepository = tokenRepository;
        _unitOfWork = unitOfWork;
        _user = user;
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
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}