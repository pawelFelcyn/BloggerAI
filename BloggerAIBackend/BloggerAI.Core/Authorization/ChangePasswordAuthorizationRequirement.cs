using BloggerAI.Core.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace BloggerAI.Core.Authorization;

public sealed class ChangePasswordAuthorizationRequirement : IAuthorizationRequirement
{
    public required ChangePasswordDto ChangePasswordDto { get; set; }
}