namespace TelegramBot.Core.ResponseBuilders.Spotify;

public interface ISpotifyAlbumResponseBuilder
{
    Task<string> BuildAsync(string albumId);
}