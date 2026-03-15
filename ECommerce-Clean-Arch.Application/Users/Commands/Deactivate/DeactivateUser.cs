using ECommerce_Clean_Arch.Application.Abstractions.Messaging;
using ECommerce_Clean_Arch.Application.Persistence;
using ECommerce_Clean_Arch.Application.Persistence.Repositories;
using ECommerce_Clean_Arch.Domain.Errors.Users;

using SharedKernel.Errors;
using SharedKernel.Results;

namespace ECommerce_Clean_Arch.Application.Users.Commands.Deactivate;

public record DeactivateUserCommand(Guid UserId) : ICommand;

public class DeactivateUserCommandHandler : ICommandHandler<DeactivateUserCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeactivateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeactivateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByIdAsync(request.UserId, cancellationToken);
        if (user is null)
        {
            return Error.NotFound(new UserNotFound(request.UserId));
        }
        user.Deactivate();

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}