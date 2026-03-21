using ECommerce_Clean_Arch.Application.Abstractions.Messaging;
using ECommerce_Clean_Arch.Application.Abstractions.Persistence;
using ECommerce_Clean_Arch.Application.Abstractions.Persistence.Repositories;
using ECommerce_Clean_Arch.Application.Authentication.Services;
using ECommerce_Clean_Arch.Application.Common.Interfaces;
using ECommerce_Clean_Arch.Application.Services;
using ECommerce_Clean_Arch.Domain.Errors.Security;
using ECommerce_Clean_Arch.Domain.Errors.Users;

using SharedKernel.Errors;
using SharedKernel.Results;

namespace ECommerce_Clean_Arch.Application.Authentication.Commands.ChangePassword;

public sealed record ChangePasswordCommand(
    string OldPassword,
    string NewPassword,
    string NewConfirmPassword
) : ICommand;

public sealed class ChangePasswordCommandHandler :
    ICommandHandler<ChangePasswordCommand>
{
    private readonly IIdentityService _identityService;
    private readonly ICookieService _cookieService;
    private readonly IUserRepository _userRepository;
    private readonly IApplicationDbContext _unitOfWork;
    private readonly IUser _user;

    public ChangePasswordCommandHandler(
        IIdentityService identityService,
        IUser user,
        IUserRepository userRepository,
        ISessionRepository tokenRepository,
        IApplicationDbContext unitOfWork,
        ICookieService cookieService
    )
    {
        _identityService = identityService;
        _user = user;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _cookieService = cookieService;
    }

    public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        // check that user id is valid from token
        var userId = _user.Id;
        if (userId is null)
        {
            var error = Error.Security(new UserUnauthenticated());
            return error;
        }

        // retrieve user
        var user = await _userRepository.GetUserByIdAsync(userId.Value, cancellationToken);
        if (user is null)
        {
            return Error.Validation(new UserNotFound(userId.Value));
        }

        // 2. update user with new password
        var changedPasswordResult = await _identityService.ChangePasswordAsync(
            user,
            request.OldPassword,
            request.NewPassword
        );
        if (changedPasswordResult.IsFailure)
        {
            return changedPasswordResult;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _cookieService.ClearRefreshToken();
        return Result.Success();
    }
}