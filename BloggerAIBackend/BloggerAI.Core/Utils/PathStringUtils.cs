using Microsoft.AspNetCore.Http;

namespace BloggerAI.Core.Utils;

public sealed class PathStringUtils : IPathStringUtils
{
    public Guid GetGuidIdOrDefault(PathString pathString)
    {
        ReadOnlySpan<char> span = pathString.Value.AsSpan().TrimEnd('/');
        int lastSlashIndex = span.LastIndexOf('/');
        ReadOnlySpan<char> lastSegment = lastSlashIndex >= 0 ? span[(lastSlashIndex + 1)..] : span;
        Guid.TryParse(lastSegment, out var id);
        return id;
    }
}