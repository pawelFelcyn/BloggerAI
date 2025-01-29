namespace BloggerAI.Core.Services;

public interface ITranscriptionService
{
    Task<string> TranscriptAudio(Stream audioStream, string filename);
}