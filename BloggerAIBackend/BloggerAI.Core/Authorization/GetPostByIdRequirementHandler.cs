using BloggerAI.Core.Services;
using BloggerAI.Core.Utils;
using BloggerAI.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BloggerAI.Core.Authorization;

internal sealed class GetPostByIdRequirementHandler : AuthorizationHandler<GetPostByIdRequirement>
{
    private readonly IUserContextService _userContextService;
    private readonly IDbContext _dbContext;
    private Guid _postId;

    public GetPostByIdRequirementHandler(IUserContextService userContextService, IDbContext dbContext,
        IHttpContextAccessor httpContextAccessor, IPathStringUtils pathStringUtils)
    {
        _userContextService = userContextService;
        _dbContext = dbContext;
        _postId = pathStringUtils.GetGuidIdOrDefault(httpContextAccessor?.HttpContext?.Request?.Path ?? default);
    }

    protected override  async Task HandleRequirementAsync(AuthorizationHandlerContext context, GetPostByIdRequirement requirement)
    {
        if ((!context.User.Identity?.IsAuthenticated) ?? true)
        {
            return;
        }

        if (context.User.IsInRole("SA"))
        {
            context.Succeed(requirement);
            return;
        }

        var authorId = await _dbContext
            .Posts
            .Where(p => p.Id == _postId)
            .Select(p => p.BloggerId)
            .FirstOrDefaultAsync();
        if (authorId == _userContextService.BloggerId || authorId == default)
        {
            context.Succeed(requirement);
        }
    }
}