using FluentAssertions;
using System.Net;

namespace BloggerAI.API.Tests.Controller;

[Collection("APICollection")]
public class PostsControllerTests
{
    private readonly APIFactory _apiFactory;

    public PostsControllerTests(APIFactory apiFactory)
    {
        _apiFactory = apiFactory;
    }

    [Fact]
    public async Task GetAll_ForAnnonymousUser_ShouldReturnUnauthorized()
    {
        var client = _apiFactory.CreateClient();
        var result = await client.GetAsync("/api/posts");
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetAll_ForAdmin_ShouldAlowGettingPostsWithoutBloggerFilterAndReturnOk()
    {
        await _apiFactory.AddBloggerWithPosts("123", 10);
        var client = _apiFactory.CreateClient();
        await _apiFactory.ApplySaAuthority(client);
        var result = await client.GetAsync("/api/posts?pageNumber=1&pageSize=30");
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetAll_ForInvalidQueryParams_ShouldReturnBadRequest()
    {
        await _apiFactory.AddBloggerWithPosts("123", 1);
        var client = _apiFactory.CreateClient();
        await _apiFactory.ApplySaAuthority(client);
        var result = await client.GetAsync("/api/posts?pageNumber=0&pageSize=0");
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetAll_ForBlogger_ShouldReturnOkWhenTriesToGetTheirsPosts()
    {
        var blogger = await _apiFactory.AddBloggerWithPosts("123", 1);
        var client = _apiFactory.CreateClient();
        await _apiFactory.ApplyAuthority(client, blogger.IdentityUser!.Email, "123");
        var result = await client.GetAsync($"/api/posts?pageNumber=1&pageSize=30&bloggerId={blogger.Id}");
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetAll_ForBlogger_SHouldReturnForbiddenWhrnTriesToGetNotTheirsPosts()
    {
        var blogger1 = await _apiFactory.AddBloggerWithPosts("123", 1);
        var blogger2 = await _apiFactory.AddUserWithBloggerRole("123");
        var client = _apiFactory.CreateClient();
        await _apiFactory.ApplyAuthority(client, blogger2.IdentityUser!.Email, "123");
        var result = await client.GetAsync($"/api/posts?pageNumber=1&pageSize=30&bloggerId={blogger1.Id}");
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task GetById_ForAnnonymousUser_ShouldReturnUnauthorized()
    {
        var client = _apiFactory.CreateClient();
        var result = await client.GetAsync($"/api/posts/{Guid.NewGuid()}");
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }


    [Fact]
    public async Task GetById_ForSuperAdmin_ShouldReturnOkWhenRequestingAnExistingPost()
    {
        var blogger = await _apiFactory.AddBloggerWithPosts("password", 1);
        var client = _apiFactory.CreateClient();
        await _apiFactory.ApplySaAuthority(client);
        var result = await client.GetAsync($"/api/posts/{blogger.Posts.First().Id}");
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetById_ForSuperAdmin_ShouldReturnNotFoundForNonExistingPost()
    {
        var client = _apiFactory.CreateClient();
        await _apiFactory.ApplySaAuthority(client);
        var result = await client.GetAsync($"/api/posts/{Guid.NewGuid()}");
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetById_ForBlogger_ShouldReturnOkWhenRequestingTheirPost()
    {
        var blogger = await _apiFactory.AddBloggerWithPosts("password", 1);
        var client = _apiFactory.CreateClient();
        await _apiFactory.ApplyAuthority(client, blogger.IdentityUser!.Email, "password");
        var result = await client.GetAsync($"/api/posts/{blogger.Posts.First().Id}");
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetById_ForBlogger_ShouldReturnNotFoundWhenRequestingNonExistingPost()
    {
        var blogger = await _apiFactory.AddUserWithBloggerRole("password");
        var client = _apiFactory.CreateClient();
        await _apiFactory.ApplyAuthority(client, blogger.IdentityUser!.Email, "password");
        var result = await client.GetAsync($"/api/posts/{Guid.NewGuid()}");
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetById_ForBlogger_ShouldReturnForbiddenWhenRequestingSbElsesPost()
    {
        var blogger = await _apiFactory.AddUserWithBloggerRole("password");
        var blogger2 = await _apiFactory.AddBloggerWithPosts("Password", 1);
        var client = _apiFactory.CreateClient();
        await _apiFactory.ApplyAuthority(client, blogger.IdentityUser!.Email, "password");
        var result = await client.GetAsync($"/api/posts/{blogger2.Posts.First().Id}");
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}