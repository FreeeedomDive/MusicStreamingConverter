using MusicSearch.Client.SpotifyClient;
using MusicSearch.Client.YandexMusicClient;
using RestSharp;

namespace MusicSearch.Client;

public class MusicSearchClient : IMusicSearchClient
{
    public MusicSearchClient(RestClient restClient)
    {
        Spotify = new SpotifyClient.SpotifyClient(restClient);
        YandexMusic = new YandexMusicClient.YandexMusicClient(restClient);
    }

    public ISpotifyClient Spotify { get; }
    public IYandexMusicClient YandexMusic { get; }
}