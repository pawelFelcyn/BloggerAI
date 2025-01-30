namespace BloggerAI.Core.Services;

public static class UserContextServiceExtensions
{
    public static Guid? GetBloggerIdOrNull(this IUserContextService userContextService)
    {
        try
        {
            return userContextService.BloggerId;
        }
        catch (InvalidOperationException)
        {
            return null;
        }
    }
}