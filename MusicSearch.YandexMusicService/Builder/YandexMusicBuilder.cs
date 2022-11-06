using System.Net;
using Yandex.Music.Api;
using Yandex.Music.Api.Common;
using YandexMusicLibrary.Auth;

namespace YandexMusicLibrary.Builder;

public class YandexMusicBuilder : IYandexMusicBuilder
{
    public YandexMusicBuilder(
        IAuthProvider authProvider
    )
    {
        this.authProvider = authProvider;
    }

    public AuthStorage BuildAuthStorage()
    {
        return new AuthStorage();
    }

    public async Task<YandexMusicApi> BuildClient(AuthStorage authStorage)
    {
        var auth = authProvider.GetAuth();
        var api = new YandexMusicApi();
        await api.User.AuthorizeAsync(authStorage, auth.Login, auth.Password);

        return api;
    }

    private readonly IAuthProvider authProvider;
}