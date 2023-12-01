using MusicSearch.Dto.Models;
using SpotifyAPI.Web;

namespace MusicSearch.SpotifyService;

public class SpotifyService : ISpotifyService
{
    public SpotifyService(ISpotifyClient spotifyClient)
    {
        this.spotifyClient = spotifyClient;
    }

    public async Task<TrackDto[]> FindTracksAsync(string query, int skip = 0, int take = 10)
    {
        var response = await spotifyClient.Search.Item(new SearchRequest(SearchRequest.Types.Track, query));
        var page = GetPage(response.Tracks.Items, skip, take);
        return page.Select(TrackToDto).ToArray();
    }

    public async Task<ArtistDto[]> FindArtistsAsync(string query, int skip = 0, int take = 10)
    {
        var response = await spotifyClient.Search.Item(new SearchRequest(SearchRequest.Types.Artist, query));
        var page = GetPage(response.Artists.Items, skip, take);
        return page.Select(ArtistToDto).ToArray()!;
    }

    public async Task<AlbumDto[]> FindAlbumsAsync(string query, int skip = 0, int take = 10)
    {
        var response = await spotifyClient.Search.Item(new SearchRequest(SearchRequest.Types.Album, query));
        var page = GetPage(response.Albums.Items, skip, take);
        return page.Select(AlbumToDto).ToArray()!;
    }

    public async Task<TrackDto> GetTrackAsync(string id)
    {
        var track = await spotifyClient.Tracks.Get(id);
        return TrackToDto(track);
    }

    public async Task<ArtistDto> GetArtistAsync(string id)
    {
        var artist = await spotifyClient.Artists.Get(id);
        return ArtistToDto(artist)!;
    }

    public async Task<AlbumDto> GetAlbumAsync(string id)
    {
        var album = await spotifyClient.Albums.Get(id);
        return AlbumToDto(album)!;
    }

    private static ArtistDto? ArtistToDto(SimpleArtist? artist)
    {
        return artist == null
            ? null
            : new ArtistDto
            {
                Id = artist.Id,
                Name = artist.Name,
                Uri = artist.Uri,
            };
    }

    private static ArtistDto? ArtistToDto(FullArtist? artist)
    {
        return artist == null
            ? null
            : new ArtistDto
            {
                Id = artist.Id,
                Name = artist.Name,
                Uri = artist.Uri,
            };
    }

    private static AlbumDto? AlbumToDto(SimpleAlbum? album)
    {
        return album == null
            ? null
            : new AlbumDto
            {
                Id = album.Id,
                Name = album.Name,
                Artist = ArtistToDto(album.Artists?.FirstOrDefault()),
                Uri = album.Uri,
            };
    }

    private static AlbumDto? AlbumToDto(FullAlbum? album)
    {
        return album == null
            ? null
            : new AlbumDto
            {
                Id = album.Id,
                Name = album.Name,
                Artist = ArtistToDto(album.Artists?.FirstOrDefault()),
                Uri = album.Uri,
            };
    }

    private static TrackDto TrackToDto(FullTrack track)
    {
        return new TrackDto
        {
            Id = track.Id,
            Title = track.Name,
            Artist = ArtistToDto(track.Artists?.FirstOrDefault()),
            Album = AlbumToDto(track.Album),
            Source = TrackSource.Spotify,
            Uri = track.Uri,
        };
    }

    private static T[] GetPage<T>(IEnumerable<T>? items, int skip, int take)
    {
        return items?.Skip(skip).Take(take).ToArray() ?? Array.Empty<T>();
    }

    private readonly ISpotifyClient spotifyClient;
}