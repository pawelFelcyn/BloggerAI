using BloggerAI.Core.Dtos;
using FluentValidation;

namespace BloggerAI.Core.Validators;

public sealed class UpdatePostDtoValidator : AbstractValidator<UpdatePostDto>
{
    public UpdatePostDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is required.")
            .MaximumLength(50)
            .WithMessage("Title cannot exceed 50 characters.");

        RuleFor(x => x.Content)
            .NotEmpty()
            .WithMessage("Content is required.")
            .MaximumLength(2000)
            .WithMessage("Content cannot exceed 2000 characters.");
    }
}