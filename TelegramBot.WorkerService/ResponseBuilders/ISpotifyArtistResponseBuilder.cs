namespace TelegramBot.WorkerService.ResponseBuilders;

public interface ISpotifyArtistResponseBuilder
{
    Task BuildAsync(long chatId, string artistId);
}