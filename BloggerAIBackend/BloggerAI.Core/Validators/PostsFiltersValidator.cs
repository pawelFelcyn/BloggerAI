using BloggerAI.Core.Dtos;
using BloggerAI.Domain;
using FluentValidation;

namespace BloggerAI.Core.Validators;

public sealed class PostsFiltersValidator : AbstractValidator<PostsFilters>
{
    public PostsFiltersValidator(IDbContext dbContext)
    {
        When(x => x.BloggerId.HasValue, () =>
        {
            RuleFor(x => x.BloggerId)
                .Must(x => dbContext.Bloggers.Any(b => b.Id == x!.Value))
                .WithMessage("This blogger does not exist.");
        });

        RuleFor(x => x.PageSize)
            .Must(x => x == 30 || x == 60 || x == 90)
            .WithMessage("Page size must be in [30, 60, 90].");

        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithMessage("Page number must be greater than 0");
    }
}