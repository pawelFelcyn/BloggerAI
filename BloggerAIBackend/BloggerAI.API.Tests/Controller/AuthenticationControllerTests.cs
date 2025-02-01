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
}