using RestSharp;
using Yandex.Music.Api.Models;

namespace MusicConverter.MusicSearch.Client.YandexMusicClient;

public class YandexMusicClient : IYandexMusicClient
{
    public YandexMusicClient(RestClient restClient)
    {
        this.restClient = restClient;
    }
    public Task<YandexTrack[]> FindTracksAsync(string query, int skip = 0, int take = 10)
    {
        throw new NotImplementedException();
    }

    public Task<YandexArtist[]> FindArtistsAsync(string query, int skip = 0, int take = 10)
    {
        throw new NotImplementedException();
    }

    public Task<YandexAlbum[]> FindAlbumsAsync(string query, int skip = 0, int take = 10)
    {
        throw new NotImplementedException();
    }

    public Task<YandexTrack> GetTrackAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task<YandexArtist> GetArtistAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task<YandexAlbum> GetAlbumAsync(string id)
    {
        throw new NotImplementedException();
    }

    private readonly RestClient restClient;
}