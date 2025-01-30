using BloggerAI.Core.Dtos;

namespace BloggerAI.Core.Services;

public interface IPostsService
{
    Task RequestCreation(Stream stream, string fileName);
    Task<PagedResult<PostDto>> GetAll(PostsFilters filters);
}