namespace TelegramBot.Core.ResponseBuilders.Spotify;

public interface ISpotifyTrackResponseBuilder
{
    Task BuildAsync(long chatId, string trackId);
}