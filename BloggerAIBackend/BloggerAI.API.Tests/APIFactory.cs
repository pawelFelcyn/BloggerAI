using BloggerAI.Core.Dtos;
using BloggerAI.Core.Services;
using BloggerAI.Domain.Entities;
using BloggerAI.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using Testcontainers.MsSql;
using IdentityUser = BloggerAI.Domain.Entities.IdentityUser;

namespace BloggerAI.API.Tests;

public class APIFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    MsSqlContainer _databaseContainer = new MsSqlBuilder().Build();

    public async Task InitializeAsync()
    {
        await _databaseContainer.StartAsync();
        Environment.SetEnvironmentVariable(
            "ConnectionStrings__DatabaseConnection", 
            _databaseContainer.GetConnectionString(), 
            EnvironmentVariableTarget.Process
            );
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        await _databaseContainer.DisposeAsync();
        Environment.SetEnvironmentVariable(
            "ConnectionStrings__DatabaseConnection",
            null,
            EnvironmentVariableTarget.Process
            );
    }

    public async Task<Blogger> AddUserWithBloggerRole(string password)
    {
        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<BloggerAIDbContext>();
        var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<IdentityUser>>();

        var bloggerRole = await dbContext.Roles.FirstAsync(r => r.Name == "Blogger");
        var identityUser = new IdentityUser
        {
            Email = $"{Guid.NewGuid()}@test.com",
            PasswordHash = "",
            Roles = [bloggerRole]
        };

        identityUser.PasswordHash = passwordHasher.HashPassword(identityUser, password);
        var blogger = new Blogger
        {
            FirstName = "John",
            LastName = "Smith",
            IdentityUser = identityUser,
        };
        dbContext.Bloggers.Add(blogger);
        await dbContext.SaveChangesAsync();
        return blogger;
    }

    public async Task<Blogger> AddBloggerWithPosts(string password, int numberOfPosts)
    {
        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<BloggerAIDbContext>();
        var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<IdentityUser>>();

        var bloggerRole = await dbContext.Roles.FirstAsync(r => r.Name == "Blogger");
        var identityUser = new IdentityUser
        {
            Email = $"{Guid.NewGuid}@test.com",
            PasswordHash = "",
            Roles = [bloggerRole]
        };

        identityUser.PasswordHash = passwordHasher.HashPassword(identityUser, password);
        var blogger = new Blogger
        {
            FirstName = "John",
            LastName = "Smith",
            IdentityUser = identityUser,
        };

        for (int i = 0; i < numberOfPosts; i++)
        {
            blogger.Posts.Add(new Post 
            {
                Title = Guid.NewGuid().ToString(),
                Content = Guid.NewGuid().ToString(),
                Format = "md"
            });
        }
        dbContext.Bloggers.Add(blogger);
        await dbContext.SaveChangesAsync();
        return blogger;
    }

    public Task ApplySaAuthority(HttpClient client) => ApplyAuthority(client, "sa@dev.com", "123");

    public async Task ApplyAuthority(HttpClient client, string email, string password)
    {
        using var scope = Services.CreateScope();
        var authorizationService = scope.ServiceProvider.GetRequiredService<IBloggerAIAuthenticationService>();
        var token = await authorizationService.GetJwtToken(new LoginDto
        {
            Email = email,
            Password = password
        });
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}