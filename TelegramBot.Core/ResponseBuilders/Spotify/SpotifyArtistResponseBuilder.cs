using System.Text;
using Core.StringComparison;
using Core.StringComparison.Extensions;
using MusicSearch.Client;
using MusicSearch.Dto.Models;
using Telegram.Bot;
using TelegramBot.Core.Extensions;

namespace TelegramBot.Core.ResponseBuilders.Spotify;

public class SpotifyArtistResponseBuilder
(
    IMusicSearchClient musicSearchClient,
    ITelegramBotClient telegramBotClient,
    IStringComparison stringComparison
) : ISpotifyArtistResponseBuilder
{
    public async Task BuildAsync(long chatId, string artistId)
    {
        var artist = await musicSearchClient.Spotify.GetArtistAsync(artistId);
        var artistInfo = ResourceToStringBuilder.SpotifyArtistToString(artist);

        var query = artist.Name;
        var searchResults = await musicSearchClient.YandexMusic.FindArtistsAsync(query);

        var sameYandexArtist = searchResults
                              .Select(x => Convert(x, artist))
                              .OrderByDescending(x => x.confidence)
                              .FirstOrDefault();
        var yandexArtistInfo = ResourceToStringBuilder.YandexMusicArtistToString(sameYandexArtist.artist, sameYandexArtist.confidence);

        await telegramBotClient.SendTextMessageAsync(
            chatId,
            new StringBuilder()
                .AppendLine(artistInfo)
                .AppendLine("===========")
                .AppendLine("Результаты поиска")
                .AppendLine(searchResults.Length.PluralizeString("результат", "результата", "результатов"))
                .AppendLine("===========")
                .AppendLine("Исполнитель в Яндекс.Музыке")
                .Append(yandexArtistInfo)
                .ToString()
        );
        if (sameYandexArtist.artist is not null)
        {
            await telegramBotClient.SendTextMessageAsync(
                chatId,
                $"https://music.yandex.ru/artist/{sameYandexArtist.artist.Id}"
            );
        }
    }

    private (ArtistDto? artist, int confidence) Convert(ArtistDto? artist, ArtistDto original)
    {
        if (artist is null)
        {
            return (null, 0);
        }

        var confidence = stringComparison.CompareArtists(original, artist);
        return (artist, confidence);
    }
}