namespace BloggerAI.Core.Services;

public interface IPostsService
{
    Task RequestCreation(Stream stream, string fileName);
}