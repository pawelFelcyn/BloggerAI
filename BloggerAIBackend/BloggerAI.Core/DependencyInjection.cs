using BloggerAI.Core.Dtos;
using BloggerAI.Core.Services;
using BloggerAI.Core.Validators;
using BloggerAI.Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace BloggerAI.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddBloggerAIServices(this IServiceCollection services)
    {
        return services.AddScoped<IPostFromTranscriptionWriter, OpenAIPostFromTranscriptionWriter>()
            .AddScoped<ITranscriptionService, OpenAITranscriptionService>()
            .AddScoped<IUserContextService, UserContextService>()
            .AddScoped<WritePostChatMessageFactory>()
            .AddScoped<IPasswordHasher<IdentityUser>, PasswordHasher<IdentityUser>>()
            .AddScoped<IBloggerAIAuthenticationService, BloggerAIAuthenticationService>()
            .AddScoped<IValidator<LoginDto>, LoginDtoValidator>();
    }
}