namespace MusicSearch.Dto.Exceptions;

public class MusicSearchYandexServiceTooManyRequestsException : MusicSearchApiException
{
    public MusicSearchYandexServiceTooManyRequestsException(Exception? baseException = null)
        : base("Яндекс при запросе просит ввести капчу, нужно немного подождать", baseException)
    {
        StatusCode = 429;
    }
}