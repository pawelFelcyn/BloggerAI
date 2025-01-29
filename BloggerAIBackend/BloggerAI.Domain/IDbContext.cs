using BloggerAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BloggerAI.Domain;

public interface IDbContext
{
    DbSet<IdentityUser> IdentityUsers { get; set; }
    DbSet<Role> Roles { get; set; }
    DbSet<Blogger> Bloggers { get; set; }
    DbSet<Post> Posts { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}