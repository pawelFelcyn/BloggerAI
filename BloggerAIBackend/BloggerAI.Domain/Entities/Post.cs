namespace BloggerAI.Domain.Entities;

public class Post
{
    public Guid Id { get; }
    public required string Title { get; set; }
    public required string Content { get; set; }
    public required string Format { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Guid BloggerId { get; set; }
    public virtual Blogger? Blogger { get; set; }
}