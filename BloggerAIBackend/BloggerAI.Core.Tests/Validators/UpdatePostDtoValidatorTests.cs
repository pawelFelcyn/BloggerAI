using BloggerAI.Core.Dtos;
using BloggerAI.Core.Services;
using BloggerAI.Core.Validators;
using FluentValidation.TestHelper;

namespace BloggerAI.Core.Tests.Validators;

public sealed class UpdatePostDtoValidatorTests(UpdatePostDtoValidator validator) : IClassFixture<UpdatePostDtoValidator>
{
    public static IEnumerable<object[]> TooLongContentTestData()
    {
        yield return new object[] { new string('x', 2001) };
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Validate_ForInvalidTitle_ReturnsProperError(string title)
    {
        var dto = new UpdatePostDto
        {
            Title = title,
            Content = "Valid content",
            Format = PostFormat.Markdown
        };
        var validationResult = validator.TestValidate(dto);
        validationResult.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Validate_ForInvalidContent_ReturnsProperError(string content)
    {
        var dto = new UpdatePostDto
        {
            Title = "Valid Title",
            Content = content,
            Format = PostFormat.Markdown
        };
        var validationResult = validator.TestValidate(dto);
        validationResult.ShouldHaveValidationErrorFor(x => x.Content);
    }

    [Theory]
    [InlineData("This is a very long title that exceeds the maximum allowed fifty characters limit for testing purposes.")]
    public void Validate_ForTooLongTitle_ReturnsProperError(string title)
    {
        var dto = new UpdatePostDto
        {
            Title = title,
            Content = "Valid content",
            Format = PostFormat.Markdown
        };
        var validationResult = validator.TestValidate(dto);
        validationResult.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Theory]
    [MemberData(nameof(TooLongContentTestData))]
    public void Validate_ForTooLongContent_ReturnsProperError(string content)
    {
        var dto = new UpdatePostDto
        {
            Title = "Valid Title",
            Content = content,
            Format = PostFormat.Markdown
        };
        var validationResult = validator.TestValidate(dto);
        validationResult.ShouldHaveValidationErrorFor(x => x.Content);
    }

    [Fact]
    public void Validate_ForValidModel_DoesNotReturnErrors()
    {
        var dto = new UpdatePostDto
        {
            Title = "Valid Title",
            Content = "Valid content",
            Format = PostFormat.Markdown
        };
        var validationResult = validator.TestValidate(dto);
        validationResult.ShouldNotHaveAnyValidationErrors();
    }
}