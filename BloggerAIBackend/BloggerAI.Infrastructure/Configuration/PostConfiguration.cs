using BloggerAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BloggerAI.Infrastructure.Configuration;

internal sealed class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Title)
            .HasMaxLength(50);
        builder.Property(x => x.Content)
            .HasMaxLength(2000);
        builder.Property(x => x.Format)
            .HasMaxLength(50);
    }
}