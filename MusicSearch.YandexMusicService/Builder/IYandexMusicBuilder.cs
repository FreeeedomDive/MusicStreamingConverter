using Yandex.Music.Api;
using Yandex.Music.Api.Common;

namespace YandexMusicLibrary.Builder;

public interface IYandexMusicBuilder
{
    AuthStorage BuildAuthStorage();
    Task<YandexMusicApi> BuildClient(AuthStorage authStorage);
}