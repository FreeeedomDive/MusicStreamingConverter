using Microsoft.AspNetCore.Mvc;
using MusicSearch.Dto.Models;
using Yandex.Music.Api.Models.Album;
using Yandex.Music.Api.Models.Artist;
using Yandex.Music.Api.Models.Search.Album;
using Yandex.Music.Api.Models.Search.Artist;
using Yandex.Music.Api.Models.Search.Track;
using Yandex.Music.Api.Models.Track;
using YandexMusicLibrary;

namespace MusicSearch.Api.Controllers;

[ApiController]
[Route("yandex")]
public class YandexMusicSearchController
{
    public YandexMusicSearchController(IYandexMusicService yandexMusicService)
    {
        this.yandexMusicService = yandexMusicService;
    }

    [HttpGet("tracks/find")]
    public async Task<TrackDto[]> FindTrack([FromQuery] string query, [FromQuery] int skip = 0, [FromQuery] int take = 10)
    {
        return await yandexMusicService.FindTracks(query, skip, take);
    }

    [HttpGet("artists/find")]
    public async Task<ArtistDto[]> FindArtists([FromQuery] string query, [FromQuery] int skip = 0, [FromQuery] int take = 10)
    {
        return await yandexMusicService.FindArtists(query, skip, take);
    }

    [HttpGet("albums/find")]
    public async Task<AlbumDto[]> FindAlbums([FromQuery] string query, [FromQuery] int skip = 0, [FromQuery] int take = 10)
    {
        return await yandexMusicService.FindAlbums(query, skip, take);
    }

    [HttpGet("tracks/{id}")]
    public async Task<TrackDto> GetTrack([FromRoute] string id)
    {
        return await yandexMusicService.GetTrack(id);
    }

    [HttpGet("artists/{id}")]
    public async Task<ArtistDto> GetArtist([FromRoute] string id)
    {
        return await yandexMusicService.GetArtist(id);
    }

    [HttpGet("albums/{id}")]
    public async Task<AlbumDto> GetAlbum([FromRoute] string id)
    {
        return await yandexMusicService.GetAlbum(id);
    }

    private readonly IYandexMusicService yandexMusicService;
}