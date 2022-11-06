﻿using System.Text.RegularExpressions;
using Loggers;
using MusicSearch.Client;
using MusicSearch.Dto.Exceptions;
using MusicSearch.Dto.Models;
using SpotifyAPI.Web;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TelegramBot.WorkerService;

public class TelegramWorker : ITelegramWorker
{
    public TelegramWorker(
        ITelegramBotClient telegramBotClient,
        IMusicSearchClient musicSearchClient,
        ILogger logger
    )
    {
        this.telegramBotClient = telegramBotClient;
        this.musicSearchClient = musicSearchClient;
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

    private Task HandlePollingErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken cts)
    {
        logger.Error(exception, "Telegram polling error");

        return Task.CompletedTask;
    }

    private async Task HandleUpdateAsync(ITelegramBotClient client, Update update, CancellationToken cts)
    {
        if (update.Message is not { Text: { } messageText } message)
            return;

        var chatId = message.Chat.Id;
        logger.Info("{Username}: {Message}", message.Chat.Username ?? chatId.ToString(), messageText);
        try
        {
            if (messageText == "/start")
            {
                await SendMessage(chatId, "Ку");
                return;
            }

            if (IsSpotifyLink(messageText, out var spotifyId) && spotifyId is not null)
            {
                await HandleSpotifyLink(chatId, spotifyId);
                return;
            }

            if (IsYandexMusicLink(messageText, out var yandexSongId) && yandexSongId is not null)
            {
                await HandleYandexMusicLink(chatId, yandexSongId);
                return;
            }

            await SendMessage(chatId, "Ничего не распарсил");
        }
        catch (Exception e)
        {
            logger.Error(e, "Exception in message handler");
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

        var query = $"{track.Artists.First().Name} {track.Name} {track.Album.Name}";
        var searchResults = await musicSearchClient.YandexMusic.FindTracksAsync(query);
        searchInfoStrings.Add(
            "Название + Исполнитель + Альбом - " +
            $"{PluralizeString(searchResults.Length, "результат", "результата", "результатов")} поиска");
        if (searchResults.Length == 0)
        {
            query = $"{track.Artists.First().Name} {track.Name}";
            searchResults = await musicSearchClient.YandexMusic.FindTracksAsync(query);
            searchInfoStrings.Add(
                "Название + Исполнитель - " +
                $"{PluralizeString(searchResults.Length, "результат", "результата", "результатов")} поиска");
        }

        var sameYandexTrack = searchResults.FirstOrDefault();
        var yandexTrackInfo = YandexMusicTrackToString(sameYandexTrack);

        await SendMessage(chatId, $"{trackInfo}\n" +
                                  $"===========\n" +
                                  $"Результаты поиска\n" +
                                  $"{string.Join("\n", searchInfoStrings)}\n" +
                                  $"===========\n" +
                                  $"Трек в Яндекс.Музыке\n" +
                                  $"{yandexTrackInfo}");
        if (sameYandexTrack is not null)
        {
            await SendMessage(chatId, 
                $"https://music.yandex.ru/album/{sameYandexTrack.Album!.Id}/track/{sameYandexTrack.Id}");
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
            $"{PluralizeString(searchResults.Length, "результат", "результата", "результатов")} поиска");

        if (searchResults.Length == 0)
        {
            query = $"{track.Artist?.Name} {track.Title}";
            searchResults = await musicSearchClient.Spotify.FindTracksAsync(query);

            searchInfoStrings.Add(
                "Название + Исполнитель - " +
                $"{PluralizeString(searchResults.Length, "результат", "результата", "результатов")} поиска");
        }

        var sameSpotifyTrack = searchResults.FirstOrDefault();
        var spotifyTrackInfo = SpotifyTrackToString(sameSpotifyTrack);

        await SendMessage(chatId, $"{trackInfo}\n" +
                                  $"===========\n" +
                                  $"Результаты поиска\n" +
                                  $"{string.Join("\n", searchInfoStrings)}\n" +
                                  $"===========\n" +
                                  $"Трек в спотифае" +
                                  $"\n{spotifyTrackInfo}");
        if (sameSpotifyTrack is not null)
        {
            await SendMessage(chatId,
                $"https://open.spotify.com/track/{sameSpotifyTrack.Uri["spotify:track:".Length..]}");
        }
    }

    private static bool IsSpotifyLink(string message, out string? id)
    {
        id = null;
        var regex = new Regex(@"https://open.spotify.com/track/(.*)");
        var hasBadSymbols = message.Contains("?si=", StringComparison.Ordinal);
        var maxLinkLength = hasBadSymbols ? message.IndexOf("?si=", StringComparison.Ordinal) : message.Length;
        var link = message[..maxLinkLength];
        var result = regex.Match(link);

        if (!result.Success)
        {
            return false;
        }

        id = result.Groups[1].Captures[0].Value;

        return true;
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

    private static string SpotifyTrackToString(FullTrack? spotifyTrack)
    {
        if (spotifyTrack == null)
        {
            return "Не нашли трек в спотифае";
        }

        return $"Исполнитель: {string.Join(" ", spotifyTrack.Artists.Select(artist => artist.Name))}\n" +
               $"Название трека: {spotifyTrack.Name}\n" +
               $"Альбом: {spotifyTrack.Album.Name}";
    }

    private static string YandexMusicTrackToString(TrackDto? yandexTrack)
    {
        if (yandexTrack == null)
        {
            return "Не нашли трек в яндекс музыке";
        }

        return $"Исполнитель: {string.Join(" ", yandexTrack.Artist?.Name)}\n" +
               $"Название трека: {yandexTrack.Title}\n" +
               $"Альбом: {string.Join(" ", yandexTrack.Album?.Name)}";
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
            logger.Error(exception, "Exception in SendMessage");
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

    private readonly ITelegramBotClient telegramBotClient;
    private readonly IMusicSearchClient musicSearchClient;
    private readonly ILogger logger;

    private readonly CancellationTokenSource cancellationTokenSource;
}