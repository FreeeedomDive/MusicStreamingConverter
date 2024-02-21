using System.Text;
using MusicSearch.Dto.Models;
using TelegramBot.Core.Extensions;
using TelegramBot.Core.Services.Match;

namespace TelegramBot.Core.ResponseBuilders.Spotify;

public class SpotifyArtistResponseBuilder(IResourceMatcher resourceMatcher) : ISpotifyArtistResponseBuilder
{
    public async Task<string> BuildAsync(string artistId)
    {
        var matchResult = await resourceMatcher.MatchArtistAsync(artistId, Source.Spotify, Source.YandexMusic);
        var spotifyArtistInfo = ResourceToStringBuilder.SpotifyArtistToString(matchResult.Original);
        var yandexArtistInfo = ResourceToStringBuilder.YandexMusicArtistToString(matchResult.MatchResult, matchResult.MatchConfidence);

        return new StringBuilder()
               .AppendLine(spotifyArtistInfo)
               .AppendLine("===========")
               .AppendLine("Результаты поиска")
               .AppendLine(matchResult.FoundResultsCount.PluralizeString("результат", "результата", "результатов"))
               .AppendLine("===========")
               .AppendLine("Исполнитель в Яндекс.Музыке")
               .AppendLine(yandexArtistInfo)
               .Append(
                   matchResult.MatchResult is not null
                       ? $"https://music.yandex.ru/artist/{matchResult.MatchResult.Id}"
                       : string.Empty
               )
               .ToString();
    }
}