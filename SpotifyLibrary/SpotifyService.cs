using SpotifyAPI.Web;

namespace SpotifyLibrary;

public class SpotifyService : ISpotifyService
{
    public SpotifyService(ISpotifyClient spotifyClient)
    {
        this.spotifyClient = spotifyClient;
    }

    public async Task<FullTrack?> FindTrack(string query)
    {
        var response = await spotifyClient.Search.Item(new SearchRequest(SearchRequest.Types.Track, query));
        return response.Tracks.Items?.FirstOrDefault();
    }

    public async Task<FullArtist?> FindArtist(string query)
    {
        var response = await spotifyClient.Search.Item(new SearchRequest(SearchRequest.Types.Artist, query));
        return response.Artists.Items?.FirstOrDefault();
    }

    public async Task<SimpleAlbum?> FindAlbum(string query)
    {
        var response = await spotifyClient.Search.Item(new SearchRequest(SearchRequest.Types.Album, query));
        return response.Albums.Items?.FirstOrDefault();
    }

    public async Task<FullTrack> GetTrack(string id)
    {
        return await spotifyClient.Tracks.Get(id);
    }

    public async Task<FullArtist> GetArtist(string id)
    {
        return await spotifyClient.Artists.Get(id);
    }

    public async Task<FullAlbum> GetAlbum(string id)
    {
        return await spotifyClient.Albums.Get(id);
    }

    private readonly ISpotifyClient spotifyClient;
}