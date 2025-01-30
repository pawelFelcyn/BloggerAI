using System.Security.Principal;

namespace BloggerAI.Core.Dtos;

public sealed class PostsFilters
{
    public Guid? BloggerId { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}