using BloggerAI.Core.Dtos;
using BloggerAI.Core.Validators;
using FluentValidation.TestHelper;

namespace BloggerAI.Core.Tests.Validators;

public sealed class ChangePasswordDtoValidatorTests
{
    private readonly ChangePasswordDtoValidator _validator = new();

    [Theory]
    [InlineData("123")]
    [InlineData("")]
    [InlineData("asdad@")]
    [InlineData("@asdad")]
    [InlineData(null)]
    public void Validate_ForInvalidEmail_ReturnsProperError(string email)
    {
        var dto = new ChangePasswordDto
        {
            Email = email,
            OldPassword = "oldPass123",
            NewPassword = "NewPass123",
            ConfirmNewPassword = "NewPass123"
        };
        var validationResult = _validator.TestValidate(dto);
        validationResult.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Validate_ForInvalidOldPassword_ReturnsProperError(string oldPassword)
    {
        var dto = new ChangePasswordDto
        {
            Email = "test@email.com",
            OldPassword = oldPassword,
            NewPassword = "NewPass123",
            ConfirmNewPassword = "NewPass123"
        };
        var validationResult = _validator.TestValidate(dto);
        validationResult.ShouldHaveValidationErrorFor(x => x.OldPassword);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("12345")]
    public void Validate_ForInvalidNewPassword_ReturnsProperError(string newPassword)
    {
        var dto = new ChangePasswordDto
        {
            Email = "test@email.com",
            OldPassword = "oldPass123",
            NewPassword = newPassword,
            ConfirmNewPassword = newPassword
        };
        var validationResult = _validator.TestValidate(dto);
        validationResult.ShouldHaveValidationErrorFor(x => x.NewPassword);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("DifferentPassword")]
    public void Validate_ForMismatchedConfirmNewPassword_ReturnsProperError(string confirmNewPassword)
    {
        var dto = new ChangePasswordDto
        {
            Email = "test@email.com",
            OldPassword = "oldPass123",
            NewPassword = "NewPass123",
            ConfirmNewPassword = confirmNewPassword
        };
        var validationResult = _validator.TestValidate(dto);
        validationResult.ShouldHaveValidationErrorFor(x => x.ConfirmNewPassword);
    }

    [Fact]
    public void Validate_ForValidModel_DoesNotReturnErrors()
    {
        var dto = new ChangePasswordDto
        {
            Email = "test@email.com",
            OldPassword = "oldPass123",
            NewPassword = "NewPass123",
            ConfirmNewPassword = "NewPass123"
        };
        var validationResult = _validator.TestValidate(dto);
        validationResult.ShouldNotHaveAnyValidationErrors();
    }
}
