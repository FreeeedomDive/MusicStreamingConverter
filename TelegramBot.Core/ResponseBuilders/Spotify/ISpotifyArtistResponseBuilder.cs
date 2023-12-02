namespace TelegramBot.Core.ResponseBuilders.Spotify;

public interface ISpotifyArtistResponseBuilder
{
    Task BuildAsync(long chatId, string artistId);
}