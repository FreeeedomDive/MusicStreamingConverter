using System.Text;
using Core.StringComparison;
using Core.StringComparison.Extensions;
using MusicSearch.Client;
using MusicSearch.Dto.Models;
using Telegram.Bot;
using TelegramBot.WorkerService.Extensions;

namespace TelegramBot.WorkerService.ResponseBuilders;

public class YandexMusicArtistResponseBuilder
(
    IMusicSearchClient musicSearchClient,
    ITelegramBotClient telegramBotClient,
    IStringComparison stringComparison
) : IYandexMusicArtistResponseBuilder
{
    public async Task BuildAsync(long chatId, string artistId)
    {
        var artist = await musicSearchClient.YandexMusic.GetArtistAsync(artistId);
        var artistInfo = ResourceToStringBuilder.YandexMusicArtistToString(artist);

        var query = artist.Name;
        var searchResults = await musicSearchClient.Spotify.FindArtistsAsync(query);

        var sameSpotifyArtist = searchResults
                               .Select(x => Convert(x, artist))
                               .OrderByDescending(x => x.confidence)
                               .FirstOrDefault();
        var spotifyArtistInfo = ResourceToStringBuilder.SpotifyArtistToString(sameSpotifyArtist.artist, sameSpotifyArtist.confidence);

        await telegramBotClient.SendTextMessageAsync(
            chatId,
            new StringBuilder()
                .AppendLine(artistInfo)
                .AppendLine("===========")
                .AppendLine("Результаты поиска")
                .AppendLine(searchResults.Length.PluralizeString("результат", "результата", "результатов"))
                .AppendLine("===========")
                .AppendLine("Исполнитель в Spotify")
                .Append(spotifyArtistInfo)
                .ToString()
        );
        if (sameSpotifyArtist.artist is not null)
        {
            await telegramBotClient.SendTextMessageAsync(
                chatId,
                $"https://open.spotify.com/artist/{sameSpotifyArtist.artist.Uri["spotify:artist:".Length..]}"
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