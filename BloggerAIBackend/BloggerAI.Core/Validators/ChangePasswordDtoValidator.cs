using BloggerAI.Core.Dtos;
using FluentValidation;

namespace BloggerAI.Core.Validators;

internal sealed class ChangePasswordDtoValidator : AbstractValidator<ChangePasswordDto>
{
    public ChangePasswordDtoValidator()
    {
        When(x => !string.IsNullOrEmpty(x.Email), () =>
        {
            RuleFor(x => x.Email)
                .EmailAddress()
                .WithMessage("This is not a valid email address");
        });

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required");

        RuleFor(x => x.OldPassword)
            .NotEmpty()
            .WithMessage("Old password is required");

        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .WithMessage("New password is required")
            .MinimumLength(6)
            .WithMessage("New password must be at least 6 characters long");

        RuleFor(x => x.ConfirmNewPassword)
            .NotEmpty()
            .WithMessage("Confirm new password is required")
            .Equal(x => x.NewPassword)
            .WithMessage("Passwords do not match");
    }
}