using MusicConverter.MusicSearch.Client.SpotifyClient;
using MusicConverter.MusicSearch.Client.YandexMusicClient;

namespace MusicConverter.MusicSearch.Client;

public interface IMusicSearchClient
{
    ISpotifyClient Spotify { get; }
    IYandexMusicClient YandexMusic { get; }
}