using Yandex.Music.Api.Models;

namespace YandexMusicLibrary;

public interface IYandexMusicService
{
    public YandexTrack? FindTrack(string query);
    public YandexArtist? FindArtist(string query);
    public YandexAlbum? FindAlbum(string query);
    
    public YandexTrack GetTrack(string id);
    public YandexArtist GetArtist(string id);
    public YandexAlbum GetAlbum(string id);
}