namespace TelegramBot.Core.ResponseBuilders.YandexMusic;

public interface IYandexMusicAlbumResponseBuilder
{
    Task<string> BuildAsync(string albumId);
}