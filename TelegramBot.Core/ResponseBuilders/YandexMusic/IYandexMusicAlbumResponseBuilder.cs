namespace TelegramBot.Core.ResponseBuilders.YandexMusic;

public interface IYandexMusicAlbumResponseBuilder
{
    Task BuildAsync(long chatId, string albumId);
}