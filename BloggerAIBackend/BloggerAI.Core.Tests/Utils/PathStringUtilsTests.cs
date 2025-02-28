using BloggerAI.Core.Utils;
using FluentAssertions;
using Microsoft.AspNetCore.Http;

namespace BloggerAI.Core.Tests.Utils;

public sealed class PathStringUtilsTests
{
    private readonly PathStringUtils _sut = new PathStringUtils();

    [Fact]
    public void GetGuidIdOrDefault_ForValidGuidAtEnd_ShouldReturnGuid()
    {
        // Arrange
        Guid expectedGuid = Guid.NewGuid();
        var path = new PathString($"/users/{expectedGuid}");

        // Act
        var result = _sut.GetGuidIdOrDefault(path);

        // Assert
        result.Should().Be(expectedGuid);
    }

    [Fact]
    public void GetGuidIdOrDefault_ForValidGuidWithTrailingSlash_ShouldReturnGuid()
    {
        // Arrange
        Guid expectedGuid = Guid.NewGuid();
        var path = new PathString($"/users/{expectedGuid}/");

        // Act
        var result = _sut.GetGuidIdOrDefault(path);

        // Assert
        result.Should().Be(expectedGuid);
    }

    [Fact]
    public void GetGuidIdOrDefault_ForPathWithoutGuid_ShouldReturnEmptyGuid()
    {
        // Arrange
        var path = new PathString("/users/profile");

        // Act
        var result = _sut.GetGuidIdOrDefault(path);

        // Assert
        result.Should().Be(Guid.Empty);
    }

    [Fact]
    public void GetGuidIdOrDefault_ForEmptyPath_ShouldReturnEmptyGuid()
    {
        // Arrange
        var path = new PathString("");

        // Act
        var result = _sut.GetGuidIdOrDefault(path);

        // Assert
        result.Should().Be(Guid.Empty);
    }

    [Fact]
    public void GetGuidIdOrDefault_ForNullPath_ShouldReturnEmptyGuid()
    {
        // Arrange
        var path = new PathString(null);

        // Act
        var result = _sut.GetGuidIdOrDefault(path);

        // Assert
        result.Should().Be(Guid.Empty);
    }

    [Fact]
    public void GetGuidIdOrDefault_ForInvalidGuidAtEnd_ShouldReturnEmptyGuid()
    {
        // Arrange
        var path = new PathString("/users/not-a-guid");

        // Act
        var result = _sut.GetGuidIdOrDefault(path);

        // Assert
        result.Should().Be(Guid.Empty);
    }

    [Fact]
    public void GetGuidIdOrDefault_ForOnlyGuid_ShouldReturnGuid()
    {
        // Arrange
        Guid expectedGuid = Guid.NewGuid();
        var path = new PathString($"/{expectedGuid}");

        // Act
        var result = _sut.GetGuidIdOrDefault(path);

        // Assert
        result.Should().Be(expectedGuid);
    }

    [Fact]
    public void GetGuidIdOrDefault_ForOnlyGuidWithTrailingSlash_ShouldReturnGuid()
    {
        // Arrange
        Guid expectedGuid = Guid.NewGuid();
        var path = new PathString($"/{expectedGuid}/");

        // Act
        var result = _sut.GetGuidIdOrDefault(path);

        // Assert
        result.Should().Be(expectedGuid);
    }

    [Fact]
    public void GetGuidIdOrDefault_ForOnlySlash_ShouldReturnEmptyGuid()
    {
        // Arrange
        var path = new PathString("/");

        // Act
        var result = _sut.GetGuidIdOrDefault(path);

        // Assert
        result.Should().Be(Guid.Empty);
    }

    [Fact]
    public void GetGuidIdOrDefault_ForMultipleTrailingSlashes_ShouldReturnEmptyGuid()
    {
        // Arrange
        var path = new PathString("/users/profile///");

        // Act
        var result = _sut.GetGuidIdOrDefault(path);

        // Assert
        result.Should().Be(Guid.Empty);
    }
}
