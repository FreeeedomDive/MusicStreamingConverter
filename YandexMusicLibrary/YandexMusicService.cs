using Yandex.Music.Api;
using Yandex.Music.Api.Models;

namespace YandexMusicLibrary;

public class YandexMusicService : IYandexMusicService
{
    public YandexMusicService(YandexApi yandexApi)
    {
        this.yandexApi = yandexApi;
    }
    
    public YandexTrack? FindTrack(string query)
    {
        var response = yandexApi.SearchTrack(query);
        return response?.FirstOrDefault();
    }

    public YandexArtist? FindArtist(string query)
    {
        var response = yandexApi.SearchArtist(query);
        return response?.FirstOrDefault();
    }

    public YandexAlbum? FindAlbum(string query)
    {
        var response = yandexApi.SearchAlbums(query);
        return response?.FirstOrDefault();
    }

    public YandexTrack GetTrack(string id)
    {
        return yandexApi.GetTrack(id);
    }

    public YandexArtist GetArtist(string id)
    {
        // в апишке нет метода поиска артиста, лол
        throw new NotImplementedException();
    }

    public YandexAlbum GetAlbum(string id)
    {
        return yandexApi.GetAlbum(id);
    }

    private readonly YandexApi yandexApi;
}