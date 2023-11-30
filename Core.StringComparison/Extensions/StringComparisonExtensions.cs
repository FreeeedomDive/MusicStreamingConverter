using MusicSearch.Dto.Models;

namespace Core.StringComparison.Extensions;

public static class StringComparisonExtensions
{
    public static int CompareTracks(this IStringComparison stringComparison, TrackDto original, TrackDto? foundTrack)
    {
        if (foundTrack == null)
        {
            return 0;
        }

        var partsComparison = new Dictionary<TrackComparisonPart, int>
        {
            [TrackComparisonPart.TrackTitle] = stringComparison.Compare(original.Title, foundTrack.Title),
            [TrackComparisonPart.ArtistName] = original.Artist != null && foundTrack.Artist != null
                ? stringComparison.Compare(original.Artist.Name, foundTrack.Artist.Name)
                : 0,
            [TrackComparisonPart.AlbumTitle] = original.Album != null && foundTrack.Album != null
                ? stringComparison.Compare(original.Album.Name, foundTrack.Album.Name)
                : 0,
        };

        var total = partsComparison.Keys
            .Select(comparisonPart => partsComparison[comparisonPart] * ComparisonPartWeights[comparisonPart])
            .Sum();

        return total / ComparisonPartWeights.Values.Sum();
    }

    private static readonly Dictionary<TrackComparisonPart, int> ComparisonPartWeights = new()
    {
        [TrackComparisonPart.TrackTitle] = 7,
        [TrackComparisonPart.ArtistName] = 7,
        [TrackComparisonPart.AlbumTitle] = 1,
    };

    private enum TrackComparisonPart
    {
        TrackTitle,
        ArtistName,
        AlbumTitle
    }
}