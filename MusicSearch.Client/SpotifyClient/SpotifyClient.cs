using MusicSearch.Client.Extensions;
using MusicSearch.Dto.Models;
using RestSharp;

namespace MusicSearch.Client.SpotifyClient;

public class SpotifyClient : ISpotifyClient
{
    public SpotifyClient(RestClient restClient)
    {
        this.restClient = restClient;
    }

    public async Task<TrackDto[]> FindTracksAsync(string query, int skip = 0, int take = 10)
    {
        var request = RequestBuilder.BuildRequest("spotify", "tracks", query, skip, take);
        var response = await restClient.ExecuteGetAsync(request);
        return response.TryDeserialize<TrackDto[]>();
    }

    public async Task<ArtistDto[]> FindArtistsAsync(string query, int skip = 0, int take = 10)
    {
        var request = RequestBuilder.BuildRequest("spotify", "artists", query, skip, take);
        var response = await restClient.ExecuteGetAsync(request);
        return response.TryDeserialize<ArtistDto[]>();
    }

    public async Task<AlbumDto[]> FindAlbumsAsync(string query, int skip = 0, int take = 10)
    {
        var request = RequestBuilder.BuildRequest("spotify", "albums", query, skip, take);
        var response = await restClient.ExecuteGetAsync(request);
        return response.TryDeserialize<AlbumDto[]>();
    }

    public async Task<TrackDto> GetTrackAsync(string id)
    {
        var request = RequestBuilder.BuildRequest("spotify", "tracks", id);
        var response = await restClient.ExecuteGetAsync(request);
        return response.TryDeserialize<TrackDto>();
    }

    public async Task<ArtistDto> GetArtistAsync(string id)
    {
        var request = RequestBuilder.BuildRequest("spotify", "artists", id);
        var response = await restClient.ExecuteGetAsync(request);
        return response.TryDeserialize<ArtistDto>();
    }

    public async Task<AlbumDto> GetAlbumAsync(string id)
    {
        var request = RequestBuilder.BuildRequest("spotify", "albums", id);
        var response = await restClient.ExecuteGetAsync(request);
        return response.TryDeserialize<AlbumDto>();
    }

    private readonly RestClient restClient;
}