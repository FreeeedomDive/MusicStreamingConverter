using Loggers;
using MusicSearch.Client.SpotifyClient;
using MusicSearch.Client.YandexMusicClient;
using RestSharp;

namespace MusicSearch.Client;

public class MusicSearchClient : IMusicSearchClient
{
    public MusicSearchClient(RestClient restClient, ILogger logger)
    {
        Spotify = new SpotifyClient.SpotifyClient(restClient);
        YandexMusic = new YandexMusicClient.YandexMusicClient(restClient, logger);
    }

    public ISpotifyClient Spotify { get; }
    public IYandexMusicClient YandexMusic { get; }
}