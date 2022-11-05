using MusicSearch.Dto.Exceptions;
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
        return GetContentWithExceptionHandling(() => GetPage(yandexApi.SearchTrack(query), skip, take));
    }

    public YandexArtist[] FindArtists(string query, int skip = 0, int take = 10)
    {
        return GetContentWithExceptionHandling(() => GetPage(yandexApi.SearchArtist(query), skip, take));
    }

    public YandexAlbum[] FindAlbums(string query, int skip = 0, int take = 10)
    {
        return GetContentWithExceptionHandling(() => GetPage(yandexApi.SearchAlbums(query), skip, take));
    }

    public YandexTrack GetTrack(string id)
    {
        return GetContentWithExceptionHandling(() => yandexApi.GetTrack(id));
    }

    public YandexArtist GetArtist(string id)
    {
        // в апишке нет метода поиска артиста, лол
        throw new NotImplementedException();
    }

    public YandexAlbum GetAlbum(string id)
    {
        return GetContentWithExceptionHandling(() => yandexApi.GetAlbum(id));
    }

    private static T GetContentWithExceptionHandling<T>(Func<T> contentFunc)
    {
        try
        {
            return contentFunc();
        }
        catch (Exception e)
        {
            if (e.Message.StartsWith("Unexpected character encountered while parsing value"))
            {
                throw new MusicSearchYandexServiceTooManyRequestsException(e);
            }
        }

        // impossible return 
        return default(T)!;
    }

    private static T[] GetPage<T>(IEnumerable<T>? items, int skip, int take)
    {
        return items?.Skip(skip).Take(take).ToArray() ?? Array.Empty<T>();
    }

    private readonly YandexApi yandexApi;
}