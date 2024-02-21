namespace TelegramBot.Core.ResponseBuilders.Spotify;

public interface ISpotifyArtistResponseBuilder
{
    Task<string> BuildAsync(string artistId);
}