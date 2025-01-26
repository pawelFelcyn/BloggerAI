using BloggerAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BloggerAI.Domain;

public interface IDbContext
{
    DbSet<IdentityUser> IdentityUsers { get; set; }
    DbSet<Role> Roles { get; set; }
}