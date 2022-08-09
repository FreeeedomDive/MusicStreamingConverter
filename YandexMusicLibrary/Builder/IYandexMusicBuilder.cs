using Yandex.Music.Api;

namespace YandexMusicLibrary.Builder;

public interface IYandexMusicBuilder
{
    YandexApi BuildClient();
}