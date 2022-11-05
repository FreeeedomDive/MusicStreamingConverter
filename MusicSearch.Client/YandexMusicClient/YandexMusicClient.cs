using Loggers;
using MusicSearch.Client.Extensions;
using RestSharp;
using Yandex.Music.Api.Models;

namespace MusicSearch.Client.YandexMusicClient;

public class YandexMusicClient : IYandexMusicClient
{
    public YandexMusicClient(RestClient restClient, ILogger logger)
    {
        this.restClient = restClient;
        this.logger = logger;
    }

    public async Task<YandexTrack[]> FindTracksAsync(string query, int skip = 0, int take = 10)
    {
        var request = RequestBuilder.BuildRequest("yandex", "tracks", query, skip, take);
        var response = await restClient.ExecuteGetAsync(request);
        return response.TryDeserialize<YandexTrack[]>(logger);
    }

    public async Task<YandexArtist[]> FindArtistsAsync(string query, int skip = 0, int take = 10)
    {
        var request = RequestBuilder.BuildRequest("yandex", "artists", query, skip, take);
        var response = await restClient.ExecuteGetAsync(request);
        return response.TryDeserialize<YandexArtist[]>(logger);
    }

    public async Task<YandexAlbum[]> FindAlbumsAsync(string query, int skip = 0, int take = 10)
    {
        var request = RequestBuilder.BuildRequest("yandex", "albums", query, skip, take);
        var response = await restClient.ExecuteGetAsync(request);
        return response.TryDeserialize<YandexAlbum[]>(logger);
    }

    public async Task<YandexTrack> GetTrackAsync(string id)
    {
        var request = RequestBuilder.BuildRequest("yandex", "tracks", id);
        var response = await restClient.ExecuteGetAsync(request);
        return response.TryDeserialize<YandexTrack>(logger);
    }

    public async Task<YandexArtist> GetArtistAsync(string id)
    {
        var request = RequestBuilder.BuildRequest("yandex", "artists", id);
        var response = await restClient.ExecuteGetAsync(request);
        return response.TryDeserialize<YandexArtist>(logger);
    }

    public async Task<YandexAlbum> GetAlbumAsync(string id)
    {
        var request = RequestBuilder.BuildRequest("yandex", "albums", id);
        var response = await restClient.ExecuteGetAsync(request);
        return response.TryDeserialize<YandexAlbum>(logger);
    }

    private readonly RestClient restClient;
    private readonly ILogger logger;
}