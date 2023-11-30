namespace TelegramBot.WorkerService.ResponseBuilders;

public interface IYandexMusicTrackResponseBuilder
{
    Task BuildAsync(long chatId, string trackId);
}