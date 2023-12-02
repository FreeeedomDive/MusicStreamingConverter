using System.Text;
using MusicSearch.Dto.Models;
using TelegramBot.Core.Extensions;
using TelegramBot.Core.Services.Match;

namespace TelegramBot.Core.ResponseBuilders.Spotify;

public class SpotifyTrackResponseBuilder(IResourceMatcher resourceMatcher) : ISpotifyTrackResponseBuilder
{
    public async Task<string> BuildAsync(string trackId)
    {
        var matchResult = await resourceMatcher.MatchTrackAsync(trackId, Source.Spotify, Source.YandexMusic);
        var spotifyTrackInfo = ResourceToStringBuilder.SpotifyTrackToString(matchResult.Original);
        var yandexTrackInfo = ResourceToStringBuilder.YandexMusicTrackToString(matchResult.MatchResult, matchResult.MatchConfidence);

        return new StringBuilder()
               .AppendLine(spotifyTrackInfo)
               .AppendLine("===========")
               .AppendLine("Результаты поиска")
               .AppendLine(matchResult.FoundResultsCount.PluralizeString("результат", "результата", "результатов"))
               .AppendLine("===========")
               .AppendLine("Трек в Яндекс.Музыке")
               .AppendLine(yandexTrackInfo)
               .Append(
                   matchResult.MatchResult is not null
                       ? $"https://music.yandex.ru/album/{matchResult.MatchResult.Album!.Id}/track/{matchResult.MatchResult.Id}"
                       : string.Empty
               )
               .ToString();
    }
}