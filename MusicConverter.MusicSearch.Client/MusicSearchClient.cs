using MusicConverter.MusicSearch.Client.SpotifyClient;
using MusicConverter.MusicSearch.Client.YandexMusicClient;
using RestSharp;

namespace MusicConverter.MusicSearch.Client;

public class MusicSearchClient : IMusicSearchClient
{
    public MusicSearchClient(RestClient restClient)
    {
        this.restClient = restClient;
        Spotify = new SpotifyClient.SpotifyClient(restClient);
        YandexMusic = new YandexMusicClient.YandexMusicClient(restClient);
    }

    public ISpotifyClient Spotify { get; }
    public IYandexMusicClient YandexMusic { get; }
    
    private readonly RestClient restClient;
}