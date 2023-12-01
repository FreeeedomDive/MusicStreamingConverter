namespace TelegramBot.WorkerService.ResponseBuilders;

public interface IYandexMusicArtistResponseBuilder
{
    Task BuildAsync(long chatId, string artistId);
}