using SpotifyAPI.Web;

namespace SpotifyLibrary;

public interface ISpotifyService
{
    public Task<FullTrack?> FindTrack(string query);
    public Task<FullArtist?> FindArtist(string query);
    public Task<SimpleAlbum?> FindAlbum(string query);
    
    public Task<FullTrack> GetTrack(string id);
    public Task<FullArtist> GetArtist(string id);
    public Task<FullAlbum> GetAlbum(string id);
}