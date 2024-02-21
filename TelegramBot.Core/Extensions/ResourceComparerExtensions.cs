using MusicSearch.Dto.Models;
using TelegramBot.Core.Models;
using TelegramBot.Core.Services.Compare;

namespace TelegramBot.Core.Extensions;

public static class ResourceComparerExtensions
{
    public static ResourceCompareResult<TrackDto>[] CompareAndOrderByConfidence(this IResourceComparer resourceComparer, TrackDto original, IEnumerable<TrackDto> others)
    {
        return others
               .Select(x => resourceComparer.Compare(original, x))
               .OrderByDescending(x => x.Confidence)
               .ToArray();
    }

    public static ResourceCompareResult<AlbumDto>[] CompareAndOrderByConfidence(this IResourceComparer resourceComparer, AlbumDto original, IEnumerable<AlbumDto> others)
    {
        return others
               .Select(x => resourceComparer.Compare(original, x))
               .OrderByDescending(x => x.Confidence)
               .ToArray();
    }

    public static ResourceCompareResult<ArtistDto>[] CompareAndOrderByConfidence(this IResourceComparer resourceComparer, ArtistDto original, IEnumerable<ArtistDto> others)
    {
        return others
               .Select(x => resourceComparer.Compare(original, x))
               .OrderByDescending(x => x.Confidence)
               .ToArray();
    }
}