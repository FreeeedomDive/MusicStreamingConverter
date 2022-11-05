using Loggers;
using MusicSearch.Client.Extensions;
using RestSharp;
using SpotifyAPI.Web;

namespace MusicSearch.Client.SpotifyClient;

public class SpotifyClient : ISpotifyClient
{
    public SpotifyClient(RestClient restClient)
    {
        this.restClient = restClient;
    }

    public async Task<FullTrack[]> FindTracksAsync(string query, int skip = 0, int take = 10)
    {
        var request = RequestBuilder.BuildRequest("spotify", "tracks", query, skip, take);
        var response = await restClient.ExecuteGetAsync(request);
        return response.TryDeserialize<FullTrack[]>();
    }

    public async Task<FullArtist[]> FindArtistsAsync(string query, int skip = 0, int take = 10)
    {
        var request = RequestBuilder.BuildRequest("spotify", "artists", query, skip, take);
        var response = await restClient.ExecuteGetAsync(request);
        return response.TryDeserialize<FullArtist[]>();
    }

    public async Task<SimpleAlbum[]> FindAlbumsAsync(string query, int skip = 0, int take = 10)
    {
        var request = RequestBuilder.BuildRequest("spotify", "albums", query, skip, take);
        var response = await restClient.ExecuteGetAsync(request);
        return response.TryDeserialize<SimpleAlbum[]>();
    }

    public async Task<FullTrack> GetTrackAsync(string id)
    {
        var request = RequestBuilder.BuildRequest("spotify", "tracks", id);
        var response = await restClient.ExecuteGetAsync(request);
        return response.TryDeserialize<FullTrack>();
    }

    public async Task<FullArtist> GetArtistAsync(string id)
    {
        var request = RequestBuilder.BuildRequest("spotify", "artists", id);
        var response = await restClient.ExecuteGetAsync(request);
        return response.TryDeserialize<FullArtist>();
    }

    public async Task<FullAlbum> GetAlbumAsync(string id)
    {
        var request = RequestBuilder.BuildRequest("spotify", "albums", id);
        var response = await restClient.ExecuteGetAsync(request);
        return response.TryDeserialize<FullAlbum>();
    }

    private readonly RestClient restClient;
}