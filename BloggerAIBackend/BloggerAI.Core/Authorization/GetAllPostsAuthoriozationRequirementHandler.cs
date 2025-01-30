using BloggerAI.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace BloggerAI.Core.Authorization;

public sealed class GetAllPostsAuthoriozationRequirementHandler : AuthorizationHandler<GetAllPostsAuthoriozationRequirement>
{
    private readonly IUserContextService _userContextService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetAllPostsAuthoriozationRequirementHandler(IUserContextService userContextService,
        IHttpContextAccessor httpContextAccessor)
    {
        _userContextService = userContextService;
        _httpContextAccessor = httpContextAccessor;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, GetAllPostsAuthoriozationRequirement requirement)
    {
        if (context.User.IsInRole("SA"))
        {
            context.Succeed(requirement);
        }

        var bloggerIdQuery = _httpContextAccessor.HttpContext.Request.Query["BloggerId"];

        if (context.User.IsInRole("Blogger") && bloggerIdQuery.Count > 0
            && bloggerIdQuery[0] == _userContextService.GetBloggerIdOrNull()?.ToString())
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}