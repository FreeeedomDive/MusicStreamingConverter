using System.Text;
using Core.StringComparison;
using Core.StringComparison.Extensions;
using MusicSearch.Client;
using MusicSearch.Dto.Models;
using Telegram.Bot;
using TelegramBot.Core.Extensions;
using TelegramBot.Core.Services.Match;

namespace TelegramBot.Core.ResponseBuilders.YandexMusic;

public class YandexMusicArtistResponseBuilder(IResourceMatcher resourceMatcher) : IYandexMusicArtistResponseBuilder
{
    public async Task<string> BuildAsync(string artistId)
    {
        var matchResult = await resourceMatcher.MatchArtistAsync(artistId, Source.YandexMusic, Source.Spotify);
        var yandexArtistInfo = ResourceToStringBuilder.YandexMusicArtistToString(matchResult.Original);
        var spotifyArtistInfo = ResourceToStringBuilder.SpotifyArtistToString(matchResult.MatchResult, matchResult.MatchConfidence);

        return new StringBuilder()
               .AppendLine(yandexArtistInfo)
               .AppendLine("===========")
               .AppendLine("Результаты поиска")
               .AppendLine(matchResult.FoundResultsCount.PluralizeString("результат", "результата", "результатов"))
               .AppendLine("===========")
               .AppendLine("Исполнитель в Spotify")
               .AppendLine(spotifyArtistInfo)
               .Append(
                   matchResult.MatchResult is not null
                       ? $"https://open.spotify.com/artist/{matchResult.MatchResult.Uri["spotify:artist:".Length..]}"
                       : string.Empty
               )
               .ToString();
    }
}