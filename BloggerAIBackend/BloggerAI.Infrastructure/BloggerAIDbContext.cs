using BloggerAI.Domain;
using BloggerAI.Domain.Entities;
using BloggerAI.Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;

namespace BloggerAI.Infrastructure;

public sealed class BloggerAIDbContext : DbContext, IDbContext
{
    public BloggerAIDbContext(DbContextOptions options) : base(options)
    {
        
    }

    public DbSet<IdentityUser> IdentityUsers { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Blogger> Bloggers { get; set; }
    public DbSet<Post> Posts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new IdentityUserConfiguration());
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        modelBuilder.ApplyConfiguration(new BloggerConfiguration());
        modelBuilder.ApplyConfiguration(new PostConfiguration());
    }
}