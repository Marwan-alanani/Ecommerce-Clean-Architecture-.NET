
namespace ECommerce_Clean_Arch.Application.Authentication.Commands.ChangePassword;

public sealed class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(c => c.NewConfirmPassword)
            .NotEmpty()
            .Matches(c => c.NewPassword)
            .WithMessage("New password doesn't match confirm password");

        RuleFor(c => c.OldPassword)
            .NotEmpty();
    }
}