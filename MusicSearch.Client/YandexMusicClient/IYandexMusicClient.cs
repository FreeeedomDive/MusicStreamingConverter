using Yandex.Music.Api.Models;

namespace MusicSearch.Client.YandexMusicClient;

public interface IYandexMusicClient
{
    public Task<YandexTrack[]> FindTracksAsync(string query, int skip = 0, int take = 10);
    public Task<YandexArtist[]> FindArtistsAsync(string query, int skip = 0, int take = 10);
    public Task<YandexAlbum[]> FindAlbumsAsync(string query, int skip = 0, int take = 10);
    
    public Task<YandexTrack> GetTrackAsync(string id);
    public Task<YandexArtist> GetArtistAsync(string id);
    public Task<YandexAlbum> GetAlbumAsync(string id);
}