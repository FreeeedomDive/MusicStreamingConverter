using System.Text;
using MusicSearch.Dto.Models;
using TelegramBot.Core.Extensions;
using TelegramBot.Core.Services.Match;

namespace TelegramBot.Core.ResponseBuilders.Spotify;

public class SpotifyAlbumResponseBuilder(IResourceMatcher resourceMatcher) : ISpotifyAlbumResponseBuilder
{
    public async Task<string> BuildAsync(string albumId)
    {
        var matchResult = await resourceMatcher.MatchAlbumAsync(albumId, Source.Spotify, Source.YandexMusic);
        var spotifyAlbumInfo = ResourceToStringBuilder.SpotifyAlbumToString(matchResult.Original);
        var yandexAlbumInfo = ResourceToStringBuilder.YandexMusicAlbumToString(matchResult.MatchResult, matchResult.MatchConfidence);

        return new StringBuilder()
               .AppendLine(spotifyAlbumInfo)
               .AppendLine("===========")
               .AppendLine("Результаты поиска")
               .AppendLine(matchResult.FoundResultsCount.PluralizeString("результат", "результата", "результатов"))
               .AppendLine("===========")
               .AppendLine("Альбом в Яндекс.Музыке")
               .AppendLine(yandexAlbumInfo)
               .Append(
                   matchResult.MatchResult is not null
                       ? $"https://music.yandex.ru/album/{matchResult.MatchResult.Id}"
                       : string.Empty
               )
               .ToString();
    }
}