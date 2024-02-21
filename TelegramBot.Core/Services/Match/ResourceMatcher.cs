using MusicSearch.Client;
using MusicSearch.Dto.Models;
using TelegramBot.Core.Extensions;
using TelegramBot.Core.Models;
using TelegramBot.Core.Services.Compare;
using TelegramBot.Core.Services.Search;

namespace TelegramBot.Core.Services.Match;

public class ResourceMatcher
(
    IMusicSearchClient musicSearchClient,
    IResourceSearcher resourceSearcher,
    IResourceComparer resourceComparer
) : IResourceMatcher
{
    public async Task<ResourceMatch<TrackDto>> MatchTrackAsync(string trackId, Source source, Source destination)
    {
        var track = await getTrackFromSource[source](trackId);
        var foundTracks = await resourceSearcher.SearchTracksAsync(track, destination);
        if (!foundTracks.Any())
        {
            return new ResourceMatch<TrackDto>
            {
                Original = track,
                MatchResult = null,
                FoundResultsCount = 0,
                MatchConfidence = 0,
            };
        }

        var compareResults = resourceComparer.CompareAndOrderByConfidence(track, foundTracks);
        var mostConfidentResult = compareResults.First();
        return new ResourceMatch<TrackDto>
        {
            Original = track,
            MatchResult = mostConfidentResult.Resource,
            FoundResultsCount = foundTracks.Length,
            MatchConfidence = mostConfidentResult.Confidence,
        };
    }

    public async Task<ResourceMatch<AlbumDto>> MatchAlbumAsync(string albumId, Source source, Source destination)
    {
        var album = await getAlbumFromSource[source](albumId);
        var foundAlbums = await resourceSearcher.SearchAlbumsAsync(album, destination);
        if (!foundAlbums.Any())
        {
            return new ResourceMatch<AlbumDto>
            {
                Original = album,
                MatchResult = null,
                FoundResultsCount = 0,
                MatchConfidence = 0,
            };
        }

        var compareResults = resourceComparer.CompareAndOrderByConfidence(album, foundAlbums);
        var mostConfidentResult = compareResults.First();
        return new ResourceMatch<AlbumDto>
        {
            Original = album,
            MatchResult = mostConfidentResult.Resource,
            FoundResultsCount = foundAlbums.Length,
            MatchConfidence = mostConfidentResult.Confidence,
        };
    }

    public async Task<ResourceMatch<ArtistDto>> MatchArtistAsync(string artistId, Source source, Source destination)
    {
        var artist = await getArtistFromSource[source](artistId);
        var foundArtists = await resourceSearcher.SearchArtistsAsync(artist, destination);
        if (!foundArtists.Any())
        {
            return new ResourceMatch<ArtistDto>
            {
                Original = artist,
                MatchResult = null,
                FoundResultsCount = 0,
                MatchConfidence = 0,
            };
        }

        var compareResults = resourceComparer.CompareAndOrderByConfidence(artist, foundArtists);
        var mostConfidentResult = compareResults.First();
        return new ResourceMatch<ArtistDto>
        {
            Original = artist,
            MatchResult = mostConfidentResult.Resource,
            FoundResultsCount = foundArtists.Length,
            MatchConfidence = mostConfidentResult.Confidence,
        };
    }

    private readonly Dictionary<Source, Func<string, Task<TrackDto>>> getTrackFromSource = new()
    {
        { Source.Spotify, id => musicSearchClient.Spotify.GetTrackAsync(id) },
        { Source.YandexMusic, id => musicSearchClient.YandexMusic.GetTrackAsync(id) },
    };

    private readonly Dictionary<Source, Func<string, Task<AlbumDto>>> getAlbumFromSource = new()
    {
        { Source.Spotify, id => musicSearchClient.Spotify.GetAlbumAsync(id) },
        { Source.YandexMusic, id => musicSearchClient.YandexMusic.GetAlbumAsync(id) },
    };

    private readonly Dictionary<Source, Func<string, Task<ArtistDto>>> getArtistFromSource = new()
    {
        { Source.Spotify, id => musicSearchClient.Spotify.GetArtistAsync(id) },
        { Source.YandexMusic, id => musicSearchClient.YandexMusic.GetArtistAsync(id) },
    };
}