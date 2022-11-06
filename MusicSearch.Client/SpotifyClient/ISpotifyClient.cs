using MusicSearch.Dto.Models;

namespace MusicSearch.Client.SpotifyClient;

public interface ISpotifyClient
{
    public Task<TrackDto[]> FindTracksAsync(string query, int skip = 0, int take = 10);
    public Task<ArtistDto[]> FindArtistsAsync(string query, int skip = 0, int take = 10);
    public Task<AlbumDto[]> FindAlbumsAsync(string query, int skip = 0, int take = 10);
    
    public Task<TrackDto> GetTrackAsync(string id);
    public Task<ArtistDto> GetArtistAsync(string id);
    public Task<AlbumDto> GetAlbumAsync(string id);
}