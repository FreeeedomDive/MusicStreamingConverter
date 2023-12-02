using MusicSearch.Dto.Models;
using TelegramBot.Core.Models;

namespace TelegramBot.Core.Services.Match;

public interface IResourceMatcher
{
    Task<ResourceMatch<TrackDto>> MatchTrackAsync(string trackId, Source source, Source destination);
    Task<ResourceMatch<AlbumDto>> MatchAlbumAsync(string albumId, Source source, Source destination);
    Task<ResourceMatch<ArtistDto>> MatchArtistAsync(string artistId, Source source, Source destination);
}