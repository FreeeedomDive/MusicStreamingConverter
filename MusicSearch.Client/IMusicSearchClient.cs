using MusicSearch.Client.SpotifyClient;
using MusicSearch.Client.YandexMusicClient;

namespace MusicSearch.Client;

public interface IMusicSearchClient
{
    ISpotifyClient Spotify { get; }
    IYandexMusicClient YandexMusic { get; }
}