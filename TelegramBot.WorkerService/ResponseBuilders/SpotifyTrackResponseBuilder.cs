using Core.StringComparison;
using Core.StringComparison.Extensions;
using MusicSearch.Client;
using MusicSearch.Dto.Models;
using Telegram.Bot;
using TelegramBot.WorkerService.Extensions;

namespace TelegramBot.WorkerService.ResponseBuilders;

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
        var trackInfo = SpotifyTrackToString(track);
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
        var yandexTrackInfo = YandexMusicTrackToString(sameYandexTrack.track, sameYandexTrack.confidence);

        await telegramBotClient.SendTextMessageAsync(
            chatId,
            $"{trackInfo}\n" +
            $"===========\n" +
            $"Результаты поиска\n" +
            $"{string.Join("\n", searchInfoStrings)}\n" +
            $"===========\n" +
            $"Трек в Яндекс.Музыке\n" +
            $"{yandexTrackInfo}"
        );
        if (sameYandexTrack.track is not null)
        {
            await telegramBotClient.SendTextMessageAsync(
                chatId,
                $"https://music.yandex.ru/album/{sameYandexTrack.track.Album!.Id}/track/{sameYandexTrack.track.Id}"
            );
        }
    }

    private static string SpotifyTrackToString(TrackDto? spotifyTrack, int? resultConfidence = null)
    {
        if (spotifyTrack == null)
        {
            return "Не нашли трек в спотифае";
        }

        return (resultConfidence == null
                   ? ""
                   : $"Уверенность в найденном результате: {resultConfidence}%\n")
               + $"Исполнитель: {string.Join(" ", spotifyTrack.Artist?.Name)}\n"
               + $"Название трека: {spotifyTrack.Title}\n"
               + $"Альбом: {spotifyTrack.Album?.Name}";
    }

    private static string YandexMusicTrackToString(TrackDto? yandexTrack, int? resultConfidence = null)
    {
        if (yandexTrack == null)
        {
            return "Не нашли трек в яндекс музыке";
        }

        return (resultConfidence == null
                   ? ""
                   : $"Уверенность в найденном результате: {resultConfidence}%\n")
               + $"Исполнитель: {string.Join(" ", yandexTrack.Artist?.Name)}\n"
               + $"Название трека: {yandexTrack.Title}\n"
               + $"Альбом: {string.Join(" ", yandexTrack.Album?.Name)}";
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