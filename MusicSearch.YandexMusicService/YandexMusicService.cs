using Yandex.Music.Api;
using Yandex.Music.Api.Models;

namespace YandexMusicLibrary;

public class YandexMusicService : IYandexMusicService
{
    public YandexMusicService(YandexApi yandexApi)
    {
        this.yandexApi = yandexApi;
    }
    
    public YandexTrack[] FindTracks(string query, int skip = 0, int take = 10)
    {
        var response = yandexApi.SearchTrack(query);
        return GetPage(response, skip, take);
    }

    public YandexArtist[] FindArtists(string query, int skip = 0, int take = 10)
    {
        var response = yandexApi.SearchArtist(query);
        return GetPage(response, skip, take);
    }

    public YandexAlbum[] FindAlbums(string query, int skip = 0, int take = 10)
    {
        var response = yandexApi.SearchAlbums(query);
        return GetPage(response, skip, take);
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
    
    private static T[] GetPage<T>(IEnumerable<T>? items, int skip, int take)
    {
        return items?.Skip(skip).Take(take).ToArray() ?? Array.Empty<T>();
    }

    private readonly YandexApi yandexApi;
}