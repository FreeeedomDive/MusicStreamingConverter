using MusicSearch.Dto.Models;

namespace YandexMusicLibrary;

public interface IYandexMusicService
{
    public Task<TrackDto[]> FindTracks(string query, int skip = 0, int take = 10);
    public Task<ArtistDto[]> FindArtists(string query, int skip = 0, int take = 10);
    public Task<AlbumDto[]> FindAlbums(string query, int skip = 0, int take = 10);
    public Task<TrackDto> GetTrack(string id);
    public Task<ArtistDto> GetArtist(string id);
    public Task<AlbumDto> GetAlbum(string id);
}