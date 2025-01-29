using Microsoft.Extensions.Configuration;
using OpenAI.Audio;

namespace BloggerAI.Core.Services;

internal class OpenAITranscriptionService : ITranscriptionService
{
    private readonly AudioClient _openaiAudioCient;

    public OpenAITranscriptionService(IConfiguration configuration)
    {
        var openAIAPIKey = configuration["Keys:OpenAIAPIKey"];
        if (openAIAPIKey == null)
        {
            throw new InvalidOperationException("You want to use an OpenAI services, but the API Key has not been configured. Consider adding API key under the configuration path: Keys -> OpenAIAPIKey");
        }
        _openaiAudioCient = new AudioClient("whisper-1", openAIAPIKey);
    }

    public async Task<string> TranscriptAudio(Stream audioStream, string filename)
    {
        AudioTranscriptionOptions options = new()
        {
            ResponseFormat = AudioTranscriptionFormat.Verbose,
            TimestampGranularities = AudioTimestampGranularities.Word | AudioTimestampGranularities.Segment
        };

        AudioTranscription transcription = await _openaiAudioCient.TranscribeAudioAsync(
            audioStream, filename, options
            );
        return transcription.Text;
    }
}