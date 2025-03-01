using BloggerAI.Core.Dtos;
using FluentAssertions;
using System.Net;

namespace BloggerAI.API.Tests.Controller;

[Collection("APICollection")]
public sealed class AuthenticationControllerTests
{
    private readonly APIFactory _apiFactory;

    public AuthenticationControllerTests(APIFactory aPIFactory)
    {
        _apiFactory = aPIFactory;
    }

    [Fact]
    public async Task Login_ValidCredentials_ReturnsOkStatusCode()
    {
        var client = _apiFactory.CreateClient();
        var blogger = await _apiFactory.AddUserWithBloggerRole("123");
        var loginDto = new LoginDto
        {
            Email = blogger.IdentityUser!.Email,
            Password = "123"
        };
        var result = await client.PostAsJsonAsync("/api/authentication/login", loginDto);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Login_ForNonexistingUser_ReturnsUnauthorizedStatusCode()
    {
        var client = _apiFactory.CreateClient();
        var loginDto = new LoginDto
        {
            Email = $"{Guid.NewGuid()}@test.com",
            Password = "123"
        };
        var result = await client.PostAsJsonAsync("/api/authentication/login", loginDto);
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Login_ValidInvalidPassword_ReturnsUnauthorizedStatusCode()
    {
        var client = _apiFactory.CreateClient();
        var blogger = await _apiFactory.AddUserWithBloggerRole("123");
        var loginDto = new LoginDto
        {
            Email = blogger.IdentityUser!.Email,
            Password = "1234"
        };
        var result = await client.PostAsJsonAsync("/api/authentication/login", loginDto);
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }


    [Fact]
    public async Task Login_ForInvalidBody_ReturnsBadRequestStatusCode()
    {
        var client = _apiFactory.CreateClient();
        var loginDto = new LoginDto
        {
            Email = "",
            Password = ""
        };
        var result = await client.PostAsJsonAsync("/api/authentication/login", loginDto);
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }


    [Fact]
    public async Task ChangePassword_ForAnonymousUser_ShouldReturnUnauthorized()
    {
        var client = _apiFactory.CreateClient();
        var dto = new ChangePasswordDto { Email = "test@email.com", OldPassword = "oldPass", NewPassword = "newPass", ConfirmNewPassword = "newPass" };

        var result = await client.PatchAsJsonAsync("/api/authentication/changePassword", dto);

        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task ChangePassword_ForUserChangingSomeoneElsesPassword_ShouldReturnForbidden()
    {
        var user1 = await _apiFactory.AddUserWithBloggerRole("password");
        var user2 = await _apiFactory.AddUserWithBloggerRole("password");
        var client = _apiFactory.CreateClient();
        await _apiFactory.ApplyAuthority(client, user1.IdentityUser!.Email, "password");
        var dto = new ChangePasswordDto { Email = user2.IdentityUser!.Email, OldPassword = "password", NewPassword = "newPass", ConfirmNewPassword = "newPass" };

        var result = await client.PatchAsJsonAsync("/api/authentication/changePassword", dto);

        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task ChangePassword_ForSuperAdmin_ShouldReturnNoContent()
    {
        var user = await _apiFactory.AddUserWithBloggerRole("password");
        var client = _apiFactory.CreateClient();
        await _apiFactory.ApplySaAuthority(client);
        var dto = new ChangePasswordDto { Email = user.IdentityUser!.Email, OldPassword = "password", NewPassword = "newPass", ConfirmNewPassword = "newPass" };

        var result = await client.PatchAsJsonAsync("/api/authentication/changePassword", dto);

        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task ChangePassword_ForInvalidBody_ShouldReturnBadRequest()
    {
        var user = await _apiFactory.AddUserWithBloggerRole("password");
        var client = _apiFactory.CreateClient();
        await _apiFactory.ApplyAuthority(client, user.IdentityUser!.Email, "password");
        var dto = new ChangePasswordDto { Email = "", OldPassword = "password", NewPassword = "newPass", ConfirmNewPassword = "newPass" };

        var result = await client.PatchAsJsonAsync("/api/authentication/changePassword", dto);

        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ChangePassword_ForNonExistingUser_ShouldReturnNotFound()
    {
        var client = _apiFactory.CreateClient();
        await _apiFactory.ApplySaAuthority(client);
        var dto = new ChangePasswordDto { Email = "nonexistent@email.com", OldPassword = "oldPass", NewPassword = "newPass", ConfirmNewPassword = "newPass" };

        var result = await client.PatchAsJsonAsync("/api/authentication/changePassword", dto);

        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ChangePassword_ForIncorrectOldPassword_ShouldReturnForbidden()
    {
        var user = await _apiFactory.AddUserWithBloggerRole("password");
        var client = _apiFactory.CreateClient();
        await _apiFactory.ApplyAuthority(client, user.IdentityUser!.Email, "password");
        var dto = new ChangePasswordDto { Email = user.IdentityUser.Email, OldPassword = "wrongPass", NewPassword = "newPass", ConfirmNewPassword = "newPass" };

        var result = await client.PatchAsJsonAsync("/api/authentication/changePassword", dto);

        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task ChangePassword_ForValidRequest_ShouldReturnNoContent()
    {
        var user = await _apiFactory.AddUserWithBloggerRole("password");
        var client = _apiFactory.CreateClient();
        await _apiFactory.ApplyAuthority(client, user.IdentityUser!.Email, "password");
        var dto = new ChangePasswordDto { Email = user.IdentityUser.Email, OldPassword = "password", NewPassword = "newPass", ConfirmNewPassword = "newPass" };

        var result = await client.PatchAsJsonAsync("/api/authentication/changePassword", dto);

        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

}