using BloggerAI.Core.Dtos;

namespace BloggerAI.Core.Services;

public interface IBloggerAIAuthenticationService
{
    Task<string> GetJwtToken(LoginDto loginDto);
}