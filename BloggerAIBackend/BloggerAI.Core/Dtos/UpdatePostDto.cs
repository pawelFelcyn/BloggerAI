using BloggerAI.Core.Services;

namespace BloggerAI.Core.Dtos;

public sealed class UpdatePostDto
{
    public required string Title { get; set; }
    public required string Content { get; set; }
    public required PostFormat Format { get; set; }
}