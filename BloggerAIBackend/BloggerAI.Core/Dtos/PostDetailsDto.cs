namespace BloggerAI.Core.Dtos;

public sealed class PostDetailsDto
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Content { get; set; }
    public required string Format { get; set; }
    public DateTime CreatedAt { get; set; }
}