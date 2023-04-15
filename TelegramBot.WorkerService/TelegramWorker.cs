using System.Text.RegularExpressions;
using Core.StringComparison;
using Core.StringComparison.Extensions;
using MusicSearch.Client;
using MusicSearch.Dto.Exceptions;
using MusicSearch.Dto.Models;
using RestSharp;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelemetryApp.Api.Client.Log;

namespace TelegramBot.WorkerService;

public class TelegramWorker : ITelegramWorker
{
    public TelegramWorker(
        ITelegramBotClient telegramBotClient,
        IMusicSearchClient musicSearchClient,
        IStringComparison stringComparison,
        ILoggerClient logger
    )
    {
        this.telegramBotClient = telegramBotClient;
        this.musicSearchClient = musicSearchClient;
        this.stringComparison = stringComparison;
        this.logger = logger;

        cancellationTokenSource = new CancellationTokenSource();
    }

    public async Task Start()
    {
        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = Array.Empty<UpdateType>()
        };

        telegramBotClient.StartReceiving(
            updateHandler: HandleUpdateAsync,
            pollingErrorHandler: HandlePollingErrorAsync,
            receiverOptions: receiverOptions,
            cancellationToken: cancellationTokenSource.Token
        );

        Console.WriteLine("Starting bot...");
        await Task.Delay(-1, cancellationTokenSource.Token);
    }

    private async Task HandlePollingErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken cts)
    {
        await logger.ErrorAsync(exception, "Telegram polling error");
    }

    private async Task HandleUpdateAsync(ITelegramBotClient client, Update update, CancellationToken cts)
    {
        if (update.Message is not { Text: { } messageText } message)
            return;

        var chatId = message.Chat.Id;
        try
        {
            if (messageText == "/start")
            {
                await SendMessage(chatId,
                    "Этот бот позволяет искать треки в стриминговых сервисах. " +
                    "Например, получив ссылку на трек в Спотифае, я попытаюсь его найти в Яндекс.Музыке и наоборот. " +
                    "Поддерживаются только эти два сервиса.");
                return;
            }

            if (IsSpotifyLink(messageText, out var spotifyId) && spotifyId is not null)
            {
                await HandleSpotifyLink(chatId, spotifyId);
                await logger.InfoAsync(
                    "Successful convert from Spotify\n{Username}: {Message}",
                    message.Chat.Username ?? chatId.ToString(), messageText
                );
                return;
            }

            if (IsYandexMusicLink(messageText, out var yandexSongId) && yandexSongId is not null)
            {
                await HandleYandexMusicLink(chatId, yandexSongId);
                await logger.InfoAsync(
                    "Successful convert from Yandex\n{Username}: {Message}",
                    message.Chat.Username ?? chatId.ToString(), messageText
                );
                return;
            }
            await logger.WarnAsync(
                "Unsuccessful convert - can not parse link\n{Username}: {Message}",
                message.Chat.Username ?? chatId.ToString(), messageText
            );
            await SendMessage(chatId, "Ничего не распарсил");
        }
        catch (Exception e)
        {
            await logger.ErrorAsync(
                e,
                "Unsuccessful convert - exception\n{Username}: {Message}",
                message.Chat.Username ?? chatId.ToString(), messageText
            );
            if (e is MusicSearchYandexServiceTooManyRequestsException)
            {
                await SendMessage(chatId,
                    $"Возникла ошибка при обработке\nСкорее всего яндекс просит ввести капчу, так что нужно подождать");
                return;
            }

            await SendMessage(chatId, $"Возникла ошибка при обработке\n{e.Message}");
        }
    }

    private async Task HandleSpotifyLink(long chatId, string songId)
    {
        var track = await musicSearchClient.Spotify.GetTrackAsync(songId);
        var trackInfo = SpotifyTrackToString(track);
        var searchInfoStrings = new List<string>();

        var query = $"{track.Artist?.Name} {track.Title} {track.Album?.Name}";
        var searchResults = await musicSearchClient.YandexMusic.FindTracksAsync(query);
        searchInfoStrings.Add(
            "Название + Исполнитель + Альбом - " +
            $"{PluralizeString(searchResults.Length, "результат", "результата", "результатов")}");
        if (searchResults.Length == 0)
        {
            query = $"{track.Artist?.Name} {track.Title}";
            searchResults = await musicSearchClient.YandexMusic.FindTracksAsync(query);
            searchInfoStrings.Add(
                "Название + Исполнитель - " +
                $"{PluralizeString(searchResults.Length, "результат", "результата", "результатов")}");
        }

        var sameYandexTrack = searchResults
            .Select(x => Convert(x, track))
            .OrderByDescending(x => x.confidence)
            .FirstOrDefault();
        var yandexTrackInfo = YandexMusicTrackToString(sameYandexTrack.track, sameYandexTrack.confidence);

        await SendMessage(chatId, $"{trackInfo}\n" +
                                  $"===========\n" +
                                  $"Результаты поиска\n" +
                                  $"{string.Join("\n", searchInfoStrings)}\n" +
                                  $"===========\n" +
                                  $"Трек в Яндекс.Музыке\n" +
                                  $"{yandexTrackInfo}");
        if (sameYandexTrack.track is not null)
        {
            await SendMessage(chatId,
                $"https://music.yandex.ru/album/{sameYandexTrack.track.Album!.Id}/track/{sameYandexTrack.track.Id}");
        }
    }

    private async Task HandleYandexMusicLink(long chatId, string songId)
    {
        var track = await musicSearchClient.YandexMusic.GetTrackAsync(songId);
        var trackInfo = YandexMusicTrackToString(track);
        var searchInfoStrings = new List<string>();

        var query = $"{track.Artist?.Name} {track.Title} {track.Album?.Name}";
        var searchResults = await musicSearchClient.Spotify.FindTracksAsync(query);

        searchInfoStrings.Add(
            "Название + Исполнитель + Альбом - " +
            $"{PluralizeString(searchResults.Length, "результат", "результата", "результатов")}");

        if (searchResults.Length == 0)
        {
            query = $"{track.Artist?.Name} {track.Title}";
            searchResults = await musicSearchClient.Spotify.FindTracksAsync(query);

            searchInfoStrings.Add(
                "Название + Исполнитель - " +
                $"{PluralizeString(searchResults.Length, "результат", "результата", "результатов")}");
        }

        var sameSpotifyTrack = searchResults
            .Select(x => Convert(x, track))
            .OrderByDescending(x => x.confidence)
            .FirstOrDefault();
        var spotifyTrackInfo = SpotifyTrackToString(sameSpotifyTrack.track, sameSpotifyTrack.confidence);

        await SendMessage(chatId, $"{trackInfo}\n" +
                                  $"===========\n" +
                                  $"Результаты поиска\n" +
                                  $"{string.Join("\n", searchInfoStrings)}\n" +
                                  $"===========\n" +
                                  $"Трек в спотифае" +
                                  $"\n{spotifyTrackInfo}");
        if (sameSpotifyTrack.track is not null)
        {
            await SendMessage(chatId,
                $"https://open.spotify.com/track/{sameSpotifyTrack.track.Uri["spotify:track:".Length..]}");
        }
    }

    private static bool IsSpotifyLink(string message, out string? id)
    {
        id = null;
        var newRegex = new Regex(@"https://spotify.link/(.*)");
        var oldRegex = new Regex(@"https://open.spotify.com/track/(.*)");
        var hasBadSymbols = message.Contains("?si=", StringComparison.Ordinal);
        var maxLinkLength = hasBadSymbols ? message.IndexOf("?si=", StringComparison.Ordinal) : message.Length;
        var link = message[..maxLinkLength];

        if (newRegex.Match(link).Success)
        {
            id = GetIdFromNewLink(link);
            return true;
        }

        var match = oldRegex.Match(link);
        if (!match.Success)
        {
            return false;
        }

        id = match.Groups[1].Captures[0].Value;
        return true;
    }

    private static string GetIdFromNewLink(string link)
    {
        var htmlResponse = new RestClient().GetAsync(new RestRequest(link)).GetAwaiter().GetResult().Content!;
        const string leftRange = "href=\"https://open.spotify.com/track/";
        const string rightRange = "?si=";
        var leftIndex = htmlResponse.IndexOf(leftRange, StringComparison.Ordinal) + leftRange.Length;
        var rightIndex = htmlResponse.IndexOf(rightRange, StringComparison.Ordinal) + rightRange.Length;
        var id = htmlResponse.Substring(leftIndex, rightIndex - leftIndex);

        return id;
    }

    private static bool IsYandexMusicLink(string message, out string? id)
    {
        id = null;
        var regex = new Regex(@"https://music.yandex.(ru|com)/album/.*/track/(\d*)(.*)");
        var result = regex.Match(message);

        if (!result.Success)
        {
            return false;
        }

        id = result.Groups[2].Captures[0].Value;

        return true;
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

    private async Task SendMessage(long chatId, string message)
    {
        try
        {
            await telegramBotClient.SendTextMessageAsync(
                chatId: chatId,
                text: message,
                cancellationToken: cancellationTokenSource.Token);
        }
        catch (Exception exception)
        {
            await logger.ErrorAsync(exception, "Exception in SendMessage");
        }
    }

    private static string PluralizeString(int count, string singleForm, string severalForm, string manyForm)
    {
        var correctCount = count % 100;
        if (correctCount is >= 10 and <= 20 || correctCount % 10 >= 5 && correctCount % 10 <= 9 ||
            correctCount % 10 == 0)
            return $"{count} {manyForm}";
        return correctCount % 10 == 1 ? $"{count} {singleForm}" : $"{count} {severalForm}";
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

    private readonly ITelegramBotClient telegramBotClient;
    private readonly IMusicSearchClient musicSearchClient;
    private readonly IStringComparison stringComparison;
    private readonly ILoggerClient logger;

    private readonly CancellationTokenSource cancellationTokenSource;
}