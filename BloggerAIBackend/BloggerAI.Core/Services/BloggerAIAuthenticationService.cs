using BloggerAI.Core.Authentication;
using BloggerAI.Core.Authorization;
using BloggerAI.Core.Dtos;
using BloggerAI.Core.Exceptions;
using BloggerAI.Core.Static;
using BloggerAI.Domain;
using BloggerAI.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
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
    private readonly IAuthorizationService _authorizationService;
    private readonly IUserContextService _userContextService;

    public BloggerAIAuthenticationService(IPasswordHasher<IdentityUser> passwordHasher,
        IDbContext dbContext, AuthenticationSettings authenticationSettings,
        IAuthorizationService authorizationService, IUserContextService userContextService)
    {
        _passwordHasher = passwordHasher;
        _dbContext = dbContext;
        _authenticationSettings = authenticationSettings;
        _authorizationService = authorizationService;
        _userContextService = userContextService;
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
            throw new UnauthorizedException();
        }

        var passwordVerivicationResult = _passwordHasher.VerifyHashedPassword(identityUser,
            identityUser.PasswordHash, loginDto.Password);

        if (passwordVerivicationResult == PasswordVerificationResult.Failed)
        {
            throw new UnauthorizedException();
        }

        List<Claim> claims = [
            new Claim(ClaimTypes.NameIdentifier, identityUser.Id.ToString()),
            new Claim(ClaimTypes.Email, identityUser.Email),
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

    public async Task ChangePassword(ChangePasswordDto changePasswordDto)
    {
        var authorizationResult = await _authorizationService.AuthorizeAsync(_userContextService.User!,
            null, new ChangePasswordAuthorizationRequirement
            {
                ChangePasswordDto = changePasswordDto
            });

        if (!authorizationResult.Succeeded)
        {
            throw new ForbiddenException();
        }

        var identityUser = _dbContext
            .IdentityUsers
            .FirstOrDefault(x => x.Email == changePasswordDto.Email);

        if (identityUser is null) 
        {
            throw new NotFoundException();
        }


        var oldPasswordVerification =
            _passwordHasher
            .VerifyHashedPassword(identityUser, identityUser.PasswordHash, changePasswordDto.OldPassword);
        
        if (oldPasswordVerification == PasswordVerificationResult.Failed) 
        {
            throw new ForbiddenException();
        }

        var newHash = _passwordHasher.HashPassword(identityUser, changePasswordDto.NewPassword);
        identityUser.PasswordHash = newHash;
        await _dbContext.SaveChangesAsync();
    }
}