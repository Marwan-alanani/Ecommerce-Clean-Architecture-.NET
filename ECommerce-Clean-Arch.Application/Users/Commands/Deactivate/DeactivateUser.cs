using ECommerce_Clean_Arch.Application.Abstractions.Messaging;
using ECommerce_Clean_Arch.Application.Abstractions.Persistence;
using ECommerce_Clean_Arch.Domain.Errors.Users;

using Microsoft.EntityFrameworkCore;

using SharedKernel.Errors;
using SharedKernel.Results;

namespace ECommerce_Clean_Arch.Application.Users.Commands.Deactivate;

public record DeactivateUserCommand(Guid UserId) : ICommand;

public class DeactivateUserCommandHandler : ICommandHandler<DeactivateUserCommand>
{
    private readonly IApplicationDbContext _context;

    public DeactivateUserCommandHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }

    public async Task<Result> Handle(DeactivateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Where(u => u.Id == request.UserId)
            .FirstOrDefaultAsync(cancellationToken);
        if (user is null)
        {
            return Error.NotFound(new UserNotFound(request.UserId));
        }

        user.Deactivate();

        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}