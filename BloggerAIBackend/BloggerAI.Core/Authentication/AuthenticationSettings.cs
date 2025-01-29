using Microsoft.Extensions.Configuration;

namespace BloggerAI.Core.Authentication;

public sealed class AuthenticationSettings
{
    public required string JwtIssuer { get; set; }
    public required string JwtKey { get; set; }
    public int JwtExpireDays { get; set; }
}