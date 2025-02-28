using Microsoft.AspNetCore.Http;

namespace BloggerAI.Core.Utils;

public interface IPathStringUtils
{
    Guid GetGuidIdOrDefault(PathString pathString);
}