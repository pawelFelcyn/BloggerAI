using BloggerAI.Domain.Entities;
using BloggerAI.Infrastructure;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
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
        dbContext.Bloggers.Add(blogger);
        await dbContext.SaveChangesAsync();
        return blogger;
    }
}