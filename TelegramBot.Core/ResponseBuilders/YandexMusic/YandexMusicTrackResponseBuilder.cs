using System.Text;
using MusicSearch.Dto.Models;
using TelegramBot.Core.Extensions;
using TelegramBot.Core.Services.Match;

namespace TelegramBot.Core.ResponseBuilders.YandexMusic;

public class YandexMusicTrackResponseBuilder(IResourceMatcher resourceMatcher) : IYandexMusicTrackResponseBuilder
{
    public async Task<string> BuildAsync(string trackId)
    {
        var matchResult = await resourceMatcher.MatchTrackAsync(trackId, Source.YandexMusic, Source.Spotify);
        var yandexTrackInfo = ResourceToStringBuilder.YandexMusicTrackToString(matchResult.Original);
        var spotifyTrackInfo = ResourceToStringBuilder.SpotifyTrackToString(matchResult.MatchResult, matchResult.MatchConfidence);

        return new StringBuilder()
               .AppendLine(yandexTrackInfo)
               .AppendLine("===========")
               .AppendLine("Результаты поиска")
               .AppendLine(matchResult.FoundResultsCount.PluralizeString("результат", "результата", "результатов"))
               .AppendLine("===========")
               .AppendLine("Трек в Spotify")
               .AppendLine(spotifyTrackInfo)
               .Append(
                   matchResult.MatchResult is not null
                       ? $"https://open.spotify.com/track/{matchResult.MatchResult.Uri["spotify:track:".Length..]}"
                       : string.Empty
               )
               .ToString();
    }
}