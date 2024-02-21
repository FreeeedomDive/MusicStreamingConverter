namespace TelegramBot.Core.ResponseBuilders.YandexMusic;

public interface IYandexMusicTrackResponseBuilder
{
    Task<string> BuildAsync(string trackId);
}