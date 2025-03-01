using BloggerAI.Core.Services;
using BloggerAI.Core.Utils;
using BloggerAI.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BloggerAI.Core.Authorization;

internal class UpdatePostByIdRequirementHandler : AuthorizationHandler<UpdatePostByIdRequirement>
{
    private readonly IUserContextService _userContextService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IDbContext _dbContext;
    private Guid _postId;

    public UpdatePostByIdRequirementHandler(IUserContextService userContextService,
        IHttpContextAccessor httpContextAccessor, IPathStringUtils pathStringUtils,
        IDbContext dbContext)
    {
        _userContextService = userContextService;
        _httpContextAccessor = httpContextAccessor;
        _dbContext = dbContext;
        _postId = pathStringUtils.GetGuidIdOrDefault(httpContextAccessor?.HttpContext?.Request?.Path ?? default);
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, UpdatePostByIdRequirement requirement)
    {
        var authorId = await _dbContext
            .Posts
            .Where(p => p.Id == _postId)
            .Select(p => p.BloggerId)
            .FirstOrDefaultAsync();

        if (context.User.IsInRole("Blogger") && (_userContextService.BloggerId == authorId || authorId == default))
        {
            context.Succeed(requirement);
        }
    }
}