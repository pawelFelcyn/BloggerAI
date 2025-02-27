using BloggerAI.Core.Dtos;
using BloggerAI.Core.Exceptions;
using BloggerAI.Domain;
using BloggerAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BloggerAI.Core.Services;

internal sealed class PostsService : IPostsService
{
    private readonly IUserContextService _userContextService;
    private readonly ITranscriptionService _transcriptionService;
    private readonly IDbContext _dbContext;
    private readonly IPostFromTranscriptionWriter _postFromTranscriptionWriter;

    public PostsService(IUserContextService userContextService,
        ITranscriptionService transcriptionService, IDbContext dbContext,
        IPostFromTranscriptionWriter postFromTranscriptionWriter)
    {
        _userContextService = userContextService;
        _transcriptionService = transcriptionService;
        _dbContext = dbContext;
        _postFromTranscriptionWriter = postFromTranscriptionWriter;
    }

    public async Task RequestCreation(Stream stream, string fileName)
    {
        if (!await _dbContext.Bloggers.AnyAsync(b => b.Id == _userContextService.BloggerId))
        {
            throw new ForbiddenException();
        }

        var _ = Task.Run(() => CreatePost(stream, fileName));
    }

    private async Task CreatePost(Stream stream, string fileName)
    {
        var transcription = await _transcriptionService.TranscriptAudio(stream, fileName);
        var postText = await _postFromTranscriptionWriter.WritePost(transcription, "pl-PL", PostFormat.Markdown);
        //TODO: properly create post here
        var post = new Post
        {
            Title = "Test title",
            Content = postText,
            Format = "md",
            CreatedAt = DateTime.UtcNow,
            BloggerId = _userContextService.BloggerId
        };

        _dbContext.Posts.Add(post);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<PagedResult<PostDto>> GetAll(PostsFilters filters)
    {
        IQueryable<Post> postsQuery = _dbContext.Posts;

        if (filters.BloggerId.HasValue)
        {
            postsQuery = postsQuery.Where(p => p.BloggerId == filters.BloggerId.Value);
        }

        var skip = (filters.PageNumber - 1) * filters.PageSize;
        postsQuery = postsQuery.Skip(skip).Take(filters.PageSize);

        var allResultsCount = await postsQuery.CountAsync();

        var dtos = await postsQuery.Select(p => new PostDto
        {
            Id = p.Id,
            Title = p.Title
        }).ToListAsync();

        var itemsFrom = skip + 1;
        return new PagedResult<PostDto>
        {
            Items = dtos,
            ItemsFrom = itemsFrom,
            ItemsTo = itemsFrom + allResultsCount - 1,
            PageNumber = filters.PageNumber,
            PageSize = filters.PageSize,
            TotalItemsCount = allResultsCount
        };
    }

    public async Task<PostDetailsDto> GetById(Guid id)
    {
        var post = await _dbContext.Posts
            .Select(p => new PostDetailsDto
            {
                Id = p.Id,
                Title = p.Title,
                Content = p.Content,
                Format = p.Format,
                CreatedAt = p.CreatedAt
            })
            .FirstOrDefaultAsync(x => x.Id == id);

        if (post is null)
        {
            throw new NotFoundException();
        }

        return post;
    }
}