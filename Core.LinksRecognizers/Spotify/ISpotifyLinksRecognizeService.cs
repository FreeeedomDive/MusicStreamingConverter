namespace Core.LinksRecognizers.Spotify;

public interface ISpotifyLinksRecognizeService
{
    Task<ResourceLink?> TryRecognizeAsync(string link);
}