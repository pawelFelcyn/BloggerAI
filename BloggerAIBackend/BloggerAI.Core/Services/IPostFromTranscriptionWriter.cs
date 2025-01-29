namespace BloggerAI.Core.Services;

public interface IPostFromTranscriptionWriter
{
    Task<string> WritePost(string transcription, string lang, PostFormat postFormat);
}
