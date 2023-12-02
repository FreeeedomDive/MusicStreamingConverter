using System.Text;
using Core.StringComparison;
using Core.StringComparison.Extensions;
using MusicSearch.Client;
using MusicSearch.Dto.Models;
using Telegram.Bot;
using TelegramBot.Core.Extensions;

namespace TelegramBot.Core.ResponseBuilders.Spotify;

public class SpotifyTrackResponseBuilder
(
    IMusicSearchClient musicSearchClient,
    ITelegramBotClient telegramBotClient,
    IStringComparison stringComparison
) : ISpotifyTrackResponseBuilder
{
    public async Task BuildAsync(long chatId, string trackId)
    {
        var track = await musicSearchClient.Spotify.GetTrackAsync(trackId);
        var trackInfo = ResourceToStringBuilder.SpotifyTrackToString(track);
        var searchInfoStrings = new List<string>();

        var query = $"{track.Artist?.Name} {track.Title} {track.Album?.Name}";
        var searchResults = await musicSearchClient.YandexMusic.FindTracksAsync(query);
        searchInfoStrings.Add(
            "Название + Исполнитель + Альбом - " +
            searchResults.Length.PluralizeString("результат", "результата", "результатов")
        );
        if (searchResults.Length == 0)
        {
            query = $"{track.Artist?.Name} {track.Title}";
            searchResults = await musicSearchClient.YandexMusic.FindTracksAsync(query);
            searchInfoStrings.Add(
                "Название + Исполнитель - " +
                searchResults.Length.PluralizeString("результат", "результата", "результатов")
            );
        }

        var sameYandexTrack = searchResults
                              .Select(x => Convert(x, track))
                              .OrderByDescending(x => x.confidence)
                              .FirstOrDefault();
        var yandexTrackInfo = ResourceToStringBuilder.YandexMusicTrackToString(sameYandexTrack.track, sameYandexTrack.confidence);

        await telegramBotClient.SendTextMessageAsync(
            chatId,
            new StringBuilder()
                .AppendLine(trackInfo)
                .AppendLine("===========")
                .AppendLine("Результаты поиска")
                .AppendLine(string.Join("\n", searchInfoStrings))
                .AppendLine("===========")
                .AppendLine("Трек в Яндекс.Музыке")
                .AppendLine(yandexTrackInfo)
                .ToString()
        );
        if (sameYandexTrack.track is not null)
        {
            await telegramBotClient.SendTextMessageAsync(
                chatId,
                $"https://music.yandex.ru/album/{sameYandexTrack.track.Album!.Id}/track/{sameYandexTrack.track.Id}"
            );
        }
    }

    private (TrackDto? track, int confidence) Convert(TrackDto? trackDto, TrackDto original)
    {
        if (trackDto is null)
        {
            return (null, 0);
        }

        var confidence = stringComparison.CompareTracks(original, trackDto);
        return (trackDto, confidence);
    }
}