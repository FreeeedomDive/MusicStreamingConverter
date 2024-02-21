namespace TelegramBot.Core.ResponseBuilders.YandexMusic;

public interface IYandexMusicArtistResponseBuilder
{
    Task<string> BuildAsync(string artistId);
}