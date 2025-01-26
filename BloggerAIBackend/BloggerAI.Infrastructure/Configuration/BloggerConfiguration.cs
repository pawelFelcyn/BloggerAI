using BloggerAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BloggerAI.Infrastructure.Configuration;

internal sealed class BloggerConfiguration : IEntityTypeConfiguration<Blogger>
{
    public void Configure(EntityTypeBuilder<Blogger> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.FirstName)
            .HasMaxLength(100);
        builder.Property(x => x.LastName)
            .HasMaxLength(100);
        builder.HasOne(x => x.IdentityUser)
            .WithOne(x => x.Blogger)
            .HasForeignKey<Blogger>(x => x.IdentityUserId);
        builder.HasMany(x => x.Posts)
            .WithOne(x => x.Blogger)
            .HasForeignKey(x => x.BloggerId);
    }
}