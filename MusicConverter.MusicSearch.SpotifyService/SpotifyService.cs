using SpotifyAPI.Web;

namespace MusicConverter.MusicSearch.SpotifyService;

public class SpotifyService : ISpotifyService
{
    public SpotifyService(ISpotifyClient spotifyClient)
    {
        this.spotifyClient = spotifyClient;
    }

    public async Task<FullTrack[]> FindTracksAsync(string query, int skip = 0, int take = 10)
    {
        var response = await spotifyClient.Search.Item(new SearchRequest(SearchRequest.Types.Track, query));
        return GetPage(response.Tracks.Items, skip, take);
    }

    public async Task<FullArtist[]> FindArtistsAsync(string query, int skip = 0, int take = 10)
    {
        var response = await spotifyClient.Search.Item(new SearchRequest(SearchRequest.Types.Artist, query));
        return GetPage(response.Artists.Items, skip, take);
    }

    public async Task<SimpleAlbum[]> FindAlbumsAsync(string query, int skip = 0, int take = 10)
    {
        var response = await spotifyClient.Search.Item(new SearchRequest(SearchRequest.Types.Album, query));
        return GetPage(response.Albums.Items, skip, take);
    }

    public async Task<FullTrack> GetTrackAsync(string id)
    {
        return await spotifyClient.Tracks.Get(id);
    }

    public async Task<FullArtist> GetArtistAsync(string id)
    {
        return await spotifyClient.Artists.Get(id);
    }

    public async Task<FullAlbum> GetAlbumAsync(string id)
    {
        return await spotifyClient.Albums.Get(id);
    }

    private static T[] GetPage<T>(IEnumerable<T>? items, int skip, int take)
    {
        return items?.Skip(skip).Take(take).ToArray() ?? Array.Empty<T>();
    }

    private readonly ISpotifyClient spotifyClient;
}