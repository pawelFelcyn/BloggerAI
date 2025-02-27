using BloggerAI.Core.Services;
using BloggerAI.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BloggerAI.Core.Authorization;

internal sealed class GetPostByIdRequirementHandler : AuthorizationHandler<GetPostByIdRequirement>
{
    private readonly IUserContextService _userContextService;
    private readonly IDbContext _dbContext;
    private Guid _postId;

    public GetPostByIdRequirementHandler(IUserContextService userContextService, IDbContext dbContext,
        IHttpContextAccessor httpContextAccessor)
    {
        _userContextService = userContextService;
        _dbContext = dbContext;
        ReadOnlySpan<char> span = httpContextAccessor.HttpContext.Request.Path.Value.AsSpan().TrimEnd('/');
        int lastSlashIndex = span.LastIndexOf('/');
        ReadOnlySpan<char> lastSegment = lastSlashIndex >= 0 ? span[(lastSlashIndex + 1)..] : span;
        Guid.TryParse(lastSegment, out _postId);
    }

    protected override  async Task HandleRequirementAsync(AuthorizationHandlerContext context, GetPostByIdRequirement requirement)
    {
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