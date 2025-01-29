using BloggerAI.Core.Static;
using Microsoft.AspNetCore.Http;

namespace BloggerAI.Core.Services;

internal sealed class UserContextService : IUserContextService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private Guid? _cachedBloggerId;

    public UserContextService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid BloggerId
    {
        get
        {
            if (_cachedBloggerId.HasValue)
            {
                return _cachedBloggerId.Value;
            }

            var bloggerIdClaim = _httpContextAccessor.HttpContext?.User?.Claims
                .FirstOrDefault(x => x.Type == ClaimsNames.BloggerId)?.Value;

            if (Guid.TryParse(bloggerIdClaim, out var bloggerId))
            {
                _cachedBloggerId = bloggerId;
                return bloggerId;
            }

            throw new InvalidOperationException("BloggerId claim not found or invalid.");
        }
    }
}