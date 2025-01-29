using BloggerAI.Core.Services;
using BloggerAI.Core.Static;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using System.Security.Claims;

namespace BloggerAI.Core.Tests.Services;

public sealed class UserContextServiceTests
{
    [Fact]
    public void BloggerId_ShouldReturnBloggerIdFromClaims_WhenValidClaimIsPresent()
    {
        // Arrange
        var bloggerId = Guid.NewGuid();
        var claims = new[]
        {
            new Claim(ClaimsNames.BloggerId, bloggerId.ToString())
        };

        var user = new ClaimsPrincipal(new ClaimsIdentity(claims));
        var httpContextAccessor = Substitute.For<IHttpContextAccessor>();
        httpContextAccessor.HttpContext.User.Returns(user);

        var userContextService = new UserContextService(httpContextAccessor);

        // Act
        var result = userContextService.BloggerId;

        // Assert
        result.Should().Be(bloggerId);
    }

    [Fact]
    public void BloggerId_ShouldCacheBloggerIdAfterFirstAccess()
    {
        // Arrange
        var bloggerId = Guid.NewGuid();
        var claims = new[]
        {
            new Claim(ClaimsNames.BloggerId, bloggerId.ToString())
        };

        var user = new ClaimsPrincipal(new ClaimsIdentity(claims));
        var httpContextAccessor = Substitute.For<IHttpContextAccessor>();
        httpContextAccessor.HttpContext.User.Returns(user);

        var userContextService = new UserContextService(httpContextAccessor);

        // Act - Access the BloggerId once and cache it
        var firstAccess = userContextService.BloggerId;

        // Now change the claim value in the mock (simulate a change)
        var newBloggerId = Guid.NewGuid();
        var updatedClaims = new[]
        {
            new Claim(ClaimsNames.BloggerId, newBloggerId.ToString())
        };
        var updatedUser = new ClaimsPrincipal(new ClaimsIdentity(updatedClaims));
        httpContextAccessor.HttpContext.User.Returns(updatedUser);

        // Access the BloggerId again after the claim has changed
        var secondAccess = userContextService.BloggerId;

        // Assert
        firstAccess.Should().Be(bloggerId);
        secondAccess.Should().Be(firstAccess);
    }

    [Fact]
    public void BloggerId_ShouldThrowException_WhenClaimIsInvalid()
    {
        // Arrange
        var claims = new[]
        {
            new Claim(ClaimsNames.BloggerId, "invalid-guid")
        };

        var user = new ClaimsPrincipal(new ClaimsIdentity(claims));
        var httpContextAccessor = Substitute.For<IHttpContextAccessor>();
        httpContextAccessor.HttpContext.User.Returns(user);

        var userContextService = new UserContextService(httpContextAccessor);

        // Act & Assert
        Action act = () =>
        {
            var _ = userContextService.BloggerId;
        };

        act.Should().Throw<InvalidOperationException>().WithMessage("BloggerId claim not found or invalid.");
    }

    [Fact]
    public void BloggerId_ShouldThrowException_WhenNoClaimPresent()
    {
        // Arrange
        var user = new ClaimsPrincipal(new ClaimsIdentity());
        var httpContextAccessor = Substitute.For<IHttpContextAccessor>();
        httpContextAccessor.HttpContext.User.Returns(user);

        var userContextService = new UserContextService(httpContextAccessor);

        // Act & Assert
        Action act = () =>
        {
            var _ = userContextService.BloggerId;
        };

        act.Should().Throw<InvalidOperationException>().WithMessage("BloggerId claim not found or invalid.");
    }
}