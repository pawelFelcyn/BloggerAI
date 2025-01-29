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
}