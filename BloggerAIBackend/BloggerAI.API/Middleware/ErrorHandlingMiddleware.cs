using BloggerAI.Core.Exceptions;

namespace BloggerAI.API.Middleware;

public class ErrorHandlingMiddleware : IMiddleware
{
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (ForbiddenException)
        {
            context.Response.StatusCode = 403;
        }
        catch (UnauthorizedException)
        {
            context.Response.StatusCode = 401;
        }
        catch (NotFoundException)
        {
            context.Response.StatusCode = 404;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error on processing request");
            throw;
        }
    }
}