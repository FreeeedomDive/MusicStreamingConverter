namespace TelegramBot.WorkerService.ResponseBuilders;

public interface IYandexMusicAlbumResponseBuilder
{
    Task BuildAsync(long chatId, string albumId);
}