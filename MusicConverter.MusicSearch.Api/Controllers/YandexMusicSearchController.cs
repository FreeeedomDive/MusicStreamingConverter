using Microsoft.AspNetCore.Mvc;
using Yandex.Music.Api.Models;
using YandexMusicLibrary;

namespace MusicConverter.MusicSearch.Api.Controllers;

[ApiController]
[Route("yandex")]
public class YandexMusicSearchController
{
    public YandexMusicSearchController(IYandexMusicService yandexMusicService)
    {
        this.yandexMusicService = yandexMusicService;
    }

    [HttpGet("tracks/find")]
    public YandexTrack[] FindTrack([FromQuery] string query, [FromQuery] int skip = 0, [FromQuery] int take = 10)
    {
        return yandexMusicService.FindTracks(query, skip, take);
    }

    [HttpGet("artists/find")]
    public YandexArtist[] FindArtists([FromQuery] string query, [FromQuery] int skip = 0, [FromQuery] int take = 10)
    {
        return yandexMusicService.FindArtists(query, skip, take);
    }

    [HttpGet("albums/find")]
    public YandexAlbum[] FindAlbums([FromQuery] string query, [FromQuery] int skip = 0, [FromQuery] int take = 10)
    {
        return yandexMusicService.FindAlbums(query, skip, take);
    }

    [HttpGet("tracks/{id}")]
    public YandexTrack GetTrack([FromRoute] string id)
    {
        return yandexMusicService.GetTrack(id);
    }

    [HttpGet("artists/{id}")]
    public YandexArtist GetArtist([FromRoute] string id)
    {
        return yandexMusicService.GetArtist(id);
    }

    [HttpGet("albums/{id}")]
    public YandexAlbum GetAlbum([FromRoute] string id)
    {
        return yandexMusicService.GetAlbum(id);
    }

    private readonly IYandexMusicService yandexMusicService;
}