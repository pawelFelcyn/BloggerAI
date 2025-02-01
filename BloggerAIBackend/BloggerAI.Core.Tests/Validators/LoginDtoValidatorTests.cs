using BloggerAI.Core.Dtos;
using BloggerAI.Core.Validators;
using FluentValidation.TestHelper;

namespace BloggerAI.Core.Tests.Validators;

public sealed class LoginDtoValidatorTests(LoginDtoValidator validator) : IClassFixture<LoginDtoValidator>
{
    [Theory]
    [InlineData("123")]
    [InlineData("")]
    [InlineData("asdad@")]
    [InlineData("@asdad")]
    [InlineData(null)]
    public void Validate_ForInvalidEmail_ReturnsProperError(string email)
    {
        var dto = new LoginDto
        {
            Email = email,
            Password = "132"
        };
        var validationResult = validator.TestValidate(dto);
        validationResult.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Validate_ForInvalidPasword_ReturnsProperError(string password)
    {
        var dto = new LoginDto
        {
            Email = "test@email.com",
            Password = password
        };
        var validationResult = validator.TestValidate(dto);
        validationResult.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Validate_ForValidModel_DoesNotReturnErrors()
    {
        var dto = new LoginDto
        {
            Email = "test@email.com",
            Password = "password"
        };
        var validationResult = validator.TestValidate(dto);
        validationResult.ShouldNotHaveAnyValidationErrors();
    }
}