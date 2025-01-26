using BloggerAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BloggerAI.Infrastructure.Configuration;

internal sealed class IdentityUserConfiguration : IEntityTypeConfiguration<IdentityUser>
{
    public void Configure(EntityTypeBuilder<IdentityUser> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Email)
            .HasMaxLength(128);
        builder.HasIndex(x => x.Email)
            .IsUnique();
        builder.HasMany(x => x.Roles)
            .WithMany(x => x.IdentityUsers);
    }
}