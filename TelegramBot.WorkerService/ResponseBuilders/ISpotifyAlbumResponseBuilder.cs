namespace TelegramBot.WorkerService.ResponseBuilders;

public interface ISpotifyAlbumResponseBuilder
{
    Task BuildAsync(long chatId, string albumId);
}