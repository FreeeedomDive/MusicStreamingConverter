namespace TelegramBot.Core.ResponseBuilders.YandexMusic;

public interface IYandexMusicTrackResponseBuilder
{
    Task BuildAsync(long chatId, string trackId);
}