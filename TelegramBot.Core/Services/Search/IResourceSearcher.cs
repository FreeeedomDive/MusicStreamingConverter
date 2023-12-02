using MusicSearch.Dto.Models;
using TelegramBot.Core.Models;

namespace TelegramBot.Core.Services.Search;

public interface IResourceSearcher
{
    Task<TrackDto[]> SearchTracksAsync(TrackDto track, Source destinationSource);
    Task<AlbumDto[]> SearchAlbumsAsync(AlbumDto album, Source destinationSource);
    Task<ArtistDto[]> SearchArtistsAsync(ArtistDto artist, Source destinationSource);
}