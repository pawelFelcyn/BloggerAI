using System.Security.Claims;

namespace BloggerAI.Core.Services;

public interface IUserContextService
{
    Guid BloggerId { get; }
    ClaimsPrincipal? User { get; }
}