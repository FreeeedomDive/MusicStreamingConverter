using Core.StringComparison;
using Core.StringComparison.Extensions;
using MusicSearch.Dto.Models;
using TelegramBot.Core.Models;

namespace TelegramBot.Core.Services.Compare;

public class ResourceComparer(IStringComparison stringComparison) : IResourceComparer
{
    public ResourceCompareResult<TrackDto> Compare(TrackDto original, TrackDto other)
    {
        return new ResourceCompareResult<TrackDto>
        {
            Resource = other,
            Confidence = stringComparison.CompareTracks(original, other),
        };
    }

    public ResourceCompareResult<AlbumDto> Compare(AlbumDto original, AlbumDto other)
    {
        return new ResourceCompareResult<AlbumDto>
        {
            Resource = other,
            Confidence = stringComparison.CompareAlbums(original, other),
        };
    }

    public ResourceCompareResult<ArtistDto> Compare(ArtistDto original, ArtistDto other)
    {
        return new ResourceCompareResult<ArtistDto>
        {
            Resource = other,
            Confidence = stringComparison.CompareArtists(original, other),
        };
    }
}