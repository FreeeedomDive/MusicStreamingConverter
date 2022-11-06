using MusicSearch.Dto.Exceptions;
using MusicSearch.Dto.Models;
using Yandex.Music.Api;
using Yandex.Music.Api.Common;
using Yandex.Music.Api.Models.Album;
using Yandex.Music.Api.Models.Artist;
using Yandex.Music.Api.Models.Search.Album;
using Yandex.Music.Api.Models.Search.Artist;
using Yandex.Music.Api.Models.Search.Track;
using Yandex.Music.Api.Models.Track;

namespace YandexMusicLibrary;

public class YandexMusicService : IYandexMusicService
{
    public YandexMusicService(
        AuthStorage authStorage,
        YandexMusicApi yandexMusicApi
    )
    {
        this.authStorage = authStorage;
        this.yandexMusicApi = yandexMusicApi;
    }

    public async Task<TrackDto[]> FindTracks(string query, int skip = 0, int take = 10)
    {
        var result = await yandexMusicApi.Search.TrackAsync(authStorage, query);

        return result.Result.Tracks.Results.Skip(skip).Take(take).Select(TrackToDto).ToArray();
    }

    public async Task<ArtistDto[]> FindArtists(string query, int skip = 0, int take = 10)
    {
        var result = await yandexMusicApi.Search.ArtistAsync(authStorage, query);
        return result.Result.Artists.Results.Skip(skip).Take(take).Select(ArtistToDto).ToArray()!;
    }

    public async Task<AlbumDto[]> FindAlbums(string query, int skip = 0, int take = 10)
    {
        var result = await yandexMusicApi.Search.AlbumsAsync(authStorage, query);
        return result.Result.Albums.Results.Skip(skip).Take(take).Select(AlbumToDto).ToArray()!;
    }

    public async Task<TrackDto> GetTrack(string id)
    {
        var result = await yandexMusicApi.Track.GetAsync(authStorage, id);
        var track = result.Result.FirstOrDefault() ?? throw new MusicSearchEntityNotFoundException(id);
        return TrackToDto(track);
    }

    public async Task<ArtistDto> GetArtist(string id)
    {
        var result = await yandexMusicApi.Artist.GetAsync(authStorage, id);
        var artist = result.Result.Artist ?? throw new MusicSearchEntityNotFoundException(id);
        return ArtistToDto(artist)!;
    }

    public async Task<AlbumDto> GetAlbum(string id)
    {
        var result = await yandexMusicApi.Album.GetAsync(authStorage, id);
        var album = result.Result ?? throw new MusicSearchEntityNotFoundException(id);
        return AlbumToDto(album)!;
    }

    private static ArtistDto? ArtistToDto(YArtist? artist)
    {
        return artist == null
            ? null
            : new ArtistDto
            {
                Id = artist.Id,
                Name = artist.Name
            };
    }

    private static AlbumDto? AlbumToDto(YAlbum? album)
    {
        return album == null
            ? null
            : new AlbumDto
            {
                Id = album.Id,
                Name = album.Title,
                Artist = ArtistToDto(album.Artists?.FirstOrDefault())
            };
    }

    private static TrackDto TrackToDto(YTrack track)
    {
        return new TrackDto
        {
            Id = track.Id,
            Title = track.Title,
            Artist = ArtistToDto(track.Artists?.FirstOrDefault()),
            Album = AlbumToDto(track.Albums?.FirstOrDefault())
        };
    }

    private static ArtistDto? ArtistToDto(YSearchArtistModel? artist)
    {
        return artist == null
            ? null
            : new ArtistDto
            {
                Id = artist.Id,
                Name = artist.Name
            };
    }

    private static AlbumDto? AlbumToDto(YSearchAlbumModel? album)
    {
        return album == null
            ? null
            : new AlbumDto
            {
                Id = album.Id,
                Name = album.Title,
                Artist = ArtistToDto(album.Artists?.FirstOrDefault())
            };
    }

    private static TrackDto TrackToDto(YSearchTrackModel track)
    {
        return new TrackDto
        {
            Id = track.Id,
            Title = track.Title,
            Artist = ArtistToDto(track.Artists?.FirstOrDefault()),
            Album = AlbumToDto(track.Albums?.FirstOrDefault())
        };
    }

    private readonly AuthStorage authStorage;
    private readonly YandexMusicApi yandexMusicApi;
}