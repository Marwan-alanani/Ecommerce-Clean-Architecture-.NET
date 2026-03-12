using FluentValidation;

namespace ECommerce_Clean_Arch.Application.Authentication.Commands.Logout;

public sealed class LogoutCommandValidator : AbstractValidator<LogoutCommand>
{
    public LogoutCommandValidator()
    {
        RuleFor(command => command.Token)
            .NotEmpty()
            .WithMessage("Refresh token is required");
    }
}