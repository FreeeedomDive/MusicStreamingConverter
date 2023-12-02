namespace TelegramBot.Core.ResponseBuilders.Spotify;

public interface ISpotifyTrackResponseBuilder
{
    Task<string> BuildAsync(string trackId);
}