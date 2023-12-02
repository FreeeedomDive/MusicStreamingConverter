using System.Text;
using MusicSearch.Dto.Models;
using TelegramBot.Core.Extensions;
using TelegramBot.Core.Services.Match;

namespace TelegramBot.Core.ResponseBuilders.YandexMusic;

public class YandexMusicAlbumResponseBuilder(IResourceMatcher resourceMatcher) : IYandexMusicAlbumResponseBuilder
{
    public async Task<string> BuildAsync(string albumId)
    {
        var matchResult = await resourceMatcher.MatchAlbumAsync(albumId, Source.YandexMusic, Source.Spotify);
        var yandexAlbumInfo = ResourceToStringBuilder.YandexMusicAlbumToString(matchResult.Original);
        var spotifyAlbumInfo = ResourceToStringBuilder.SpotifyAlbumToString(matchResult.MatchResult, matchResult.MatchConfidence);

        return new StringBuilder()
               .AppendLine(yandexAlbumInfo)
               .AppendLine("===========")
               .AppendLine("Результаты поиска")
               .AppendLine(matchResult.FoundResultsCount.PluralizeString("результат", "результата", "результатов"))
               .AppendLine("===========")
               .AppendLine("Альбом в Spotify")
               .AppendLine(spotifyAlbumInfo)
               .Append(
                   matchResult.MatchResult is not null
                       ? $"https://open.spotify.com/album/{matchResult.MatchResult.Uri["spotify:album:".Length..]}"
                       : string.Empty
               )
               .ToString();
    }
}