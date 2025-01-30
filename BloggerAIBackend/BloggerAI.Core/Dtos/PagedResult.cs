using System.Security.Principal;

namespace BloggerAI.Core.Dtos;

public sealed class PagedResult<T>
{
    public required  List<T> Items { get; set; }
    public int ItemsFrom { get; set; }
    public int ItemsTo { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItemsCount { get; set; }
}