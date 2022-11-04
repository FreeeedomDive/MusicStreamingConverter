using Yandex.Music.Api.Models;

namespace YandexMusicLibrary;

public interface IYandexMusicService
{
    public YandexTrack[] FindTracks(string query, int skip = 0, int take = 10);
    public YandexArtist[] FindArtists(string query, int skip = 0, int take = 10);
    public YandexAlbum[] FindAlbums(string query, int skip = 0, int take = 10);
    
    public YandexTrack GetTrack(string id);
    public YandexArtist GetArtist(string id);
    public YandexAlbum GetAlbum(string id);
}