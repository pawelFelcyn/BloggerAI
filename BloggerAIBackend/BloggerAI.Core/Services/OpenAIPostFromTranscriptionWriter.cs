
using Microsoft.Extensions.Configuration;
using OpenAI.Chat;

namespace BloggerAI.Core.Services;

public sealed class OpenAIPostFromTranscriptionWriter : IPostFromTranscriptionWriter
{
    private readonly ChatClient _chatClient;
    private readonly WritePostChatMessageFactory _writePostChatMessageFactory;

    public OpenAIPostFromTranscriptionWriter(IConfiguration configuration,
        WritePostChatMessageFactory writePostChatMessageFactory)
    {
        var openAIAPIKey = configuration["Keys:OpenAIAPIKey"];
        if (openAIAPIKey == null)
        {
            throw new InvalidOperationException("You want to use an OpenAI services, but the API Key has not been configured. Consider adding API key under the configuration path: Keys -> OpenAIAPIKey");
        }
        _chatClient = new("gpt-4", openAIAPIKey);
        _writePostChatMessageFactory = writePostChatMessageFactory;
    }

    public async Task<string> WritePost(string transcription, string lang, PostFormat postFormat)
    {
        var message = _writePostChatMessageFactory.GetChatMessage(transcription, lang, postFormat);
        var result = await _chatClient.CompleteChatAsync(message);
        return result.Value.Content.ToString() ?? "";
    }
}