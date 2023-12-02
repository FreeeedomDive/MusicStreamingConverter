namespace TelegramBot.Core.ResponseBuilders.YandexMusic;

public interface IYandexMusicArtistResponseBuilder
{
    Task BuildAsync(long chatId, string artistId);
}