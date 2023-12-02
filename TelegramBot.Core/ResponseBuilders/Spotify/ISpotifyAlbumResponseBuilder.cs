namespace TelegramBot.Core.ResponseBuilders.Spotify;

public interface ISpotifyAlbumResponseBuilder
{
    Task BuildAsync(long chatId, string albumId);
}