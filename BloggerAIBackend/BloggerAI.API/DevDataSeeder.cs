﻿using BloggerAI.Domain;
using BloggerAI.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using IdentityUser = BloggerAI.Domain.Entities.IdentityUser;

namespace BloggerAI.API;

public sealed class DevDataSeeder
{
    private readonly IDbContext _dbContext;
    private readonly IPasswordHasher<IdentityUser> _passwordHasher;

    public DevDataSeeder(IDbContext dbContext, IPasswordHasher<IdentityUser> passwordHasher)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
    }

    public async Task SeedDevelopmentEnvironmentData()
    {
        if (!await _dbContext.Bloggers.AnyAsync())
        {
            var bloggerRole = _dbContext.Roles.First(r => r.Name == "Blogger");
            var identityUser = new IdentityUser
            {
                Email = "blogger@dev.com",
                PasswordHash = ""
            };
            identityUser.Roles.Add(bloggerRole);
            var passwordHash = _passwordHasher.HashPassword(identityUser, "123");
            identityUser.PasswordHash = passwordHash;
            await _dbContext.Bloggers.AddAsync(new Blogger 
            {
                FirstName = "John",
                LastName = "Smith",
                IdentityUser = identityUser,
            });
            await _dbContext.SaveChangesAsync();
        }
    }
}