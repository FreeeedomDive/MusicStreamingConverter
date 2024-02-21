using MusicSearch.Dto.Models;
using TelegramBot.Core.Models;

namespace TelegramBot.Core.Services.Compare;

public interface IResourceComparer
{
    ResourceCompareResult<TrackDto> Compare(TrackDto original, TrackDto other);
    ResourceCompareResult<AlbumDto> Compare(AlbumDto original, AlbumDto other);
    ResourceCompareResult<ArtistDto> Compare(ArtistDto original, ArtistDto other);
}