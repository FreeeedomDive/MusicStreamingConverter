namespace MusicSearch.Dto.Exceptions;

public abstract class MusicSearchApiException : Exception
{
    public MusicSearchApiException(string? message, Exception? baseException) : base(message, baseException)
    {
        
    }
    
    public int StatusCode { get; protected set; }
}