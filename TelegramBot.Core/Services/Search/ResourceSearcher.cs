using MusicSearch.Client;
using MusicSearch.Dto.Models;

namespace TelegramBot.Core.Services.Search;

public class ResourceSearcher(IMusicSearchClient musicSearchClient) : IResourceSearcher
{
    public async Task<TrackDto[]> SearchTracksAsync(TrackDto track, Source destinationSource)
    {
        var query = $"{track.Artist?.Name} {track.Title} {track.Album?.Name}";
        var foundTracksWithAlbum = await searchTracksFromSource[destinationSource](query);
        query = $"{track.Artist?.Name} {track.Title}";
        var foundTracksWithoutAlbum = await searchTracksFromSource[destinationSource](query);
        var result = foundTracksWithAlbum
                     .Concat(foundTracksWithoutAlbum)
                     .DistinctBy(x => x.Id)
                     .ToArray();
        return result;
    }

    public async Task<AlbumDto[]> SearchAlbumsAsync(AlbumDto album, Source destinationSource)
    {
        var query = $"{album.Artist?.Name} {album.Name}";
        var result = await searchAlbumsFromSource[destinationSource](query);
        return result;
    }

    public async Task<ArtistDto[]> SearchArtistsAsync(ArtistDto artist, Source destinationSource)
    {
        var query = artist.Name;
        var result = await searchArtistsFromSource[destinationSource](query);
        return result;
    }

    // TODO: to client extensions?
    private readonly Dictionary<Source, Func<string, Task<TrackDto[]>>> searchTracksFromSource = new()
    {
        { Source.Spotify, query => musicSearchClient.Spotify.FindTracksAsync(query) },
        { Source.YandexMusic, query => musicSearchClient.YandexMusic.FindTracksAsync(query) },
    };

    private readonly Dictionary<Source, Func<string, Task<AlbumDto[]>>> searchAlbumsFromSource = new()
    {
        { Source.Spotify, query => musicSearchClient.Spotify.FindAlbumsAsync(query) },
        { Source.YandexMusic, query => musicSearchClient.YandexMusic.FindAlbumsAsync(query) },
    };

    private readonly Dictionary<Source, Func<string, Task<ArtistDto[]>>> searchArtistsFromSource = new()
    {
        { Source.Spotify, query => musicSearchClient.Spotify.FindArtistsAsync(query) },
        { Source.YandexMusic, query => musicSearchClient.YandexMusic.FindArtistsAsync(query) },
    };
}