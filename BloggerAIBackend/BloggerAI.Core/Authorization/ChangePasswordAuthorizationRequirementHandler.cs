using BloggerAI.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BloggerAI.Core.Authorization;

internal sealed class ChangePasswordAuthorizationRequirementHandler : AuthorizationHandler<ChangePasswordAuthorizationRequirement>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ChangePasswordAuthorizationRequirementHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ChangePasswordAuthorizationRequirement requirement)
    {
        if (_httpContextAccessor.HttpContext.User.IsInRole("SA")
            || _httpContextAccessor.HttpContext.User.HasClaim(c => c.Type == ClaimTypes.Email && c.Value == requirement.ChangePasswordDto.Email))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}