namespace MusicSearch.Dto.Exceptions;

public class MusicSearchEntityNotFoundException : MusicSearchApiException
{
    public MusicSearchEntityNotFoundException(string id, Exception? baseException = null)
        : base($"Entity {id} not found", baseException)
    {
        StatusCode = 404;
    }
}