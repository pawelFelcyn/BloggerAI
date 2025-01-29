using BloggerAI.Core.Authentication;
using BloggerAI.Core.Dtos;
using BloggerAI.Core.Static;
using BloggerAI.Domain;
using BloggerAI.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BloggerAI.Core.Services;

public sealed class BloggerAIAuthenticationService : IBloggerAIAuthenticationService
{
    private readonly IPasswordHasher<IdentityUser> _passwordHasher;
    private readonly IDbContext _dbContext;
    private readonly AuthenticationSettings _authenticationSettings;

    public BloggerAIAuthenticationService(IPasswordHasher<IdentityUser> passwordHasher,
        IDbContext dbContext, AuthenticationSettings authenticationSettings)
    {
        _passwordHasher = passwordHasher;
        _dbContext = dbContext;
        _authenticationSettings = authenticationSettings;
    }

    public async Task<string> GetJwtToken(LoginDto loginDto)
    {
        var identityUser = await _dbContext
            .IdentityUsers
            .Include(i => i.Roles)
            .Include(i => i.Blogger)
            .FirstOrDefaultAsync(i => i.Email == loginDto.Email);
        if (identityUser is null)
        {
            throw new UnauthorizedAccessException();
        }

        var passwordVerivicationResult = _passwordHasher.VerifyHashedPassword(identityUser,
            identityUser.PasswordHash, loginDto.Password);

        if (passwordVerivicationResult == PasswordVerificationResult.Failed)
        {
            throw new UnauthorizedAccessException();
        }

        List<Claim> claims = [
            new Claim(ClaimTypes.NameIdentifier, identityUser.Id.ToString()),
            ];

        foreach (var role in identityUser.Roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role.Name));
        }

        if (identityUser.Blogger != null)
        {
            claims.Add(new Claim(ClaimsNames.BloggerId, identityUser.Blogger.Id.ToString()));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddDays(_authenticationSettings.JwtExpireDays);

        var tokenDescriptor = new JwtSecurityToken(
            issuer: _authenticationSettings.JwtIssuer,
            audience: _authenticationSettings.JwtIssuer,
            claims: claims,
            expires: expires,
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }
}