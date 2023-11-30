namespace TelegramBot.WorkerService.ResponseBuilders;

public interface ISpotifyTrackResponseBuilder
{
    Task BuildAsync(long chatId, string trackId);
}