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
        // now it works without auth
        // var auth = authProvider.GetAuth();
        // var api = new YandexMusicApi();
        // await api.User.AuthorizeAsync(authStorage, auth.Login, auth.Password);

        return new YandexMusicApi();
    }

    private readonly IAuthProvider authProvider;
}