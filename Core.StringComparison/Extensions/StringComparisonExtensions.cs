using MusicSearch.Dto.Models;

namespace Core.StringComparison.Extensions;

public static class StringComparisonExtensions
{
    public static int CompareTracks(this IStringComparison stringComparison, TrackDto original, TrackDto foundTrack)
    {
        var result = 0;

        result += stringComparison.Compare(original.Title, foundTrack.Title);

        if (original.Artist != null && foundTrack.Artist != null)
        {
            result += stringComparison.Compare(original.Artist.Name, foundTrack.Artist.Name);
        }

        if (original.Album != null && foundTrack.Album != null)
        {
            result += stringComparison.Compare(original.Album.Name, foundTrack.Album.Name);
        }

        return result / 3;
    }
}