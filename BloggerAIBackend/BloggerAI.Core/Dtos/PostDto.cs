namespace BloggerAI.Core.Dtos;

public sealed class PostDto
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
}