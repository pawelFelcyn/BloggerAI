using BloggerAI.Core.Dtos;
using BloggerAI.Core.Validators;
using BloggerAI.Domain;
using BloggerAI.Domain.Entities;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using NSubstitute;

namespace BloggerAI.Core.Tests.Validators;

public sealed class PostsFiltersValidatorTests
{
    private readonly IDbContext _dbContextMock;
    private readonly PostsFiltersValidator _validator;

    public PostsFiltersValidatorTests()
    {
        _dbContextMock = Substitute.For<IDbContext>();

        var existingBlogger = new Blogger { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe" };
        var bloggers = new List<Blogger> { existingBlogger }.AsQueryable();

        var bloggersDbSetMock = Substitute.For<DbSet<Blogger>, IQueryable<Blogger>>();
        ((IQueryable<Blogger>)bloggersDbSetMock).Provider.Returns(bloggers.Provider);
        ((IQueryable<Blogger>)bloggersDbSetMock).Expression.Returns(bloggers.Expression);
        ((IQueryable<Blogger>)bloggersDbSetMock).ElementType.Returns(bloggers.ElementType);
        ((IQueryable<Blogger>)bloggersDbSetMock).GetEnumerator().Returns(bloggers.GetEnumerator());

        _dbContextMock.Bloggers.Returns(bloggersDbSetMock);

        _validator = new PostsFiltersValidator(_dbContextMock);
    }

    [Fact]
    public void Validate_ForNonExistingBloggerId_ReturnsError()
    {
        var filters = new PostsFilters
        {
            BloggerId = Guid.NewGuid(),
            PageNumber = 1,
            PageSize = 30
        };

        var result = _validator.TestValidate(filters);
        result.ShouldHaveValidationErrorFor(x => x.BloggerId);
    }

    [Fact]
    public void Validate_ForExistingBloggerId_DoesNotReturnError()
    {
        var existingBloggerId = _dbContextMock.Bloggers.First().Id;

        var filters = new PostsFilters
        {
            BloggerId = existingBloggerId,
            PageNumber = 1,
            PageSize = 30
        };

        var result = _validator.TestValidate(filters);
        result.ShouldNotHaveValidationErrorFor(x => x.BloggerId);
    }

    [Theory]
    [InlineData(10)]
    [InlineData(45)]
    [InlineData(100)]
    public void Validate_ForInvalidPageSize_ReturnsError(int pageSize)
    {
        var filters = new PostsFilters
        {
            PageSize = pageSize,
            PageNumber = 1
        };

        var result = _validator.TestValidate(filters);
        result.ShouldHaveValidationErrorFor(x => x.PageSize);
    }

    [Theory]
    [InlineData(30)]
    [InlineData(60)]
    [InlineData(90)]
    public void Validate_ForValidPageSize_DoesNotReturnError(int pageSize)
    {
        var filters = new PostsFilters
        {
            PageSize = pageSize,
            PageNumber = 1
        };

        var result = _validator.TestValidate(filters);
        result.ShouldNotHaveValidationErrorFor(x => x.PageSize);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-10)]
    public void Validate_ForInvalidPageNumber_ReturnsError(int pageNumber)
    {
        var filters = new PostsFilters
        {
            PageSize = 30,
            PageNumber = pageNumber
        };

        var result = _validator.TestValidate(filters);
        result.ShouldHaveValidationErrorFor(x => x.PageNumber);
    }

    [Fact]
    public void Validate_ForValidModel_DoesNotReturnAnyErrors()
    {
        var existingBloggerId = _dbContextMock.Bloggers.First().Id;

        var filters = new PostsFilters
        {
            BloggerId = existingBloggerId,
            PageSize = 60,
            PageNumber = 2
        };

        var result = _validator.TestValidate(filters);
        result.ShouldNotHaveAnyValidationErrors();
    }
}