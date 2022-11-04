using Microsoft.AspNetCore.Mvc;
using MusicConverter.MusicSearch.SpotifyService;
using SpotifyAPI.Web;

namespace MusicConverter.MusicSearch.Api.Controllers;

[ApiController]
[Route("spotify")]
public class SpotifySearchController : ControllerBase
{
    public SpotifySearchController(ISpotifyService spotifyService)
    {
        this.spotifyService = spotifyService;
    }

    [HttpGet("tracks/find")]
    public async Task<FullTrack[]> FindTrack(
        [FromQuery] string query,
        [FromQuery] int skip = 0,
        [FromQuery] int take = 10
    )
    {
        return await spotifyService.FindTracksAsync(query, skip, take);
    }

    [HttpGet("artists/find")]
    public async Task<FullArtist[]> FindArtistsAsync(
        [FromQuery] string query,
        [FromQuery] int skip = 0,
        [FromQuery] int take = 10
    )
    {
        return await spotifyService.FindArtistsAsync(query, skip, take);
    }

    [HttpGet("albums/find")]
    public async Task<SimpleAlbum[]> FindAlbumsAsync(
        [FromQuery] string query,
        [FromQuery] int skip = 0,
        [FromQuery] int take = 10
    )
    {
        return await spotifyService.FindAlbumsAsync(query, skip, take);
    }

    [HttpGet("tracks/{id}")]
    public async Task<FullTrack> GetTrackAsync([FromRoute] string id)
    {
        return await spotifyService.GetTrackAsync(id);
    }

    [HttpGet("artists/{id}")]
    public async Task<FullArtist> GetArtistAsync([FromRoute] string id)
    {
        return await spotifyService.GetArtistAsync(id);
    }

    [HttpGet("albums/{id}")]
    public async Task<FullAlbum> GetAlbumAsync([FromRoute] string id)
    {
        return await spotifyService.GetAlbumAsync(id);
    }
    
    private readonly ISpotifyService spotifyService;
}