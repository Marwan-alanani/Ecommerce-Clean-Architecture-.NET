using FluentValidation;

namespace ECommerce_Clean_Arch.Application.Authentication.Commands.LogoutAllSessions;

public sealed class LogoutAllSessionsCommandValidator : AbstractValidator<LogoutAllSessionsCommand>
{
    public LogoutAllSessionsCommandValidator()
    {
        RuleFor(c => c.UserId)
            .Must(id => Guid.TryParse(id, out _))
            .WithMessage("Invalid user id");
    }
}