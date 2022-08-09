using System.Net;
using Yandex.Music.Api;
using YandexMusicLibrary.Auth;

namespace YandexMusicLibrary.Builder;

public class YandexMusicBuilder : IYandexMusicBuilder
{
    public YandexMusicBuilder(IAuthProvider authProvider)
    {
        this.authProvider = authProvider;
    }

    public YandexApi BuildClient()
    {
        var auth = authProvider.GetAuth();
        var api = new YandexMusicApi();
        api.Authorize(auth.Login, auth.Password);

        return api;
    }

    private readonly IAuthProvider authProvider;
}