using System.Text.RegularExpressions;
using SpotifyAPI.Web;
using SpotifyLibrary;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Yandex.Music.Api.Models;
using YandexMusicLibrary;

namespace TelegramBot;

public class TelegramWorker : ITelegramWorker
{
    public TelegramWorker(
        ITelegramBotClient telegramBotClient,
        ISpotifyService spotifyService,
        IYandexMusicService yandexMusicService
    )
    {
        this.telegramBotClient = telegramBotClient;
        this.spotifyService = spotifyService;
        this.yandexMusicService = yandexMusicService;

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
        Console.WriteLine(exception.Message);

        return Task.CompletedTask;
    }

    private async Task HandleUpdateAsync(ITelegramBotClient client, Update update, CancellationToken cts)
    {
        if (update.Message is not { Text: { } messageText } message)
            return;

        var chatId = message.Chat.Id;

        if (messageText == "/start")
        {
            await telegramBotClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Ку",
                cancellationToken: cancellationTokenSource.Token);
            return;
        }

        if (IsSpotifyLink(messageText, out var spotifyId))
        {
            await HandleSpotifyLink(chatId, spotifyId);
            return;
        }

        if (IsYandexMusicLink(messageText, out var yandexSongId))
        {
            await HandleYandexMusicLink(chatId, yandexSongId);
            return;
        }

        await SendMessage(chatId, "Ничего не распарсил");
    }

    private async Task HandleSpotifyLink(long chatId, string songId)
    {
        var track = await spotifyService.GetTrack(songId);
        var trackInfo = SpotifyTrackToString(track);
        var query = $"{track.Artists.First().Name} {track.Name} {track.Album.Name}";

        var sameYandexTrack = yandexMusicService.FindTrack(query);
        var yandexTrackInfo = YandexMusicTrackToString(sameYandexTrack);

        await SendMessage(chatId, $"{trackInfo}\n===========\nТрек в Яндекс.Музыке\n{yandexTrackInfo}");
        if (sameYandexTrack is not null)
        {
            await SendMessage(chatId, $"https://music.yandex.ru/album/{sameYandexTrack.Albums.First().Id}/track/{sameYandexTrack.Id}");
        }
    }

    private async Task HandleYandexMusicLink(long chatId, string songId)
    {
        var track = yandexMusicService.GetTrack(songId);
        var trackInfo = YandexMusicTrackToString(track);
        var query = $"{track.Artists.First().Name} {track.Title} {track.Albums.First().Title}";
        var sameSpotifyTrack = await spotifyService.FindTrack(query);
        var spotifyTrackInfo = SpotifyTrackToString(sameSpotifyTrack);

        await SendMessage(chatId, $"{trackInfo}\n===========\nТрек в спотифае\n{spotifyTrackInfo}");
        if (sameSpotifyTrack is not null)
        {
            await SendMessage(chatId, $"https://open.spotify.com/track/{sameSpotifyTrack.Uri["spotify:track:".Length..]}");
        }
    }

    private static bool IsSpotifyLink(string message, out string id)
    {
        id = null;
        var regex = new Regex(@"https://open.spotify.com/track/(.*)(\?si=.*)?");
        var result = regex.Match(message);

        if (!result.Success)
        {
            return false;
        }

        id = result.Groups[1].Captures[0].Value;

        return true;
    }

    public static bool IsYandexMusicLink(string message, out string id)
    {
        id = null;
        var regex = new Regex(@"https://music.yandex.ru/album/.*/track/(.*)");
        var result = regex.Match(message);

        if (!result.Success)
        {
            return false;
        }

        id = result.Groups[1].Captures[0].Value;

        return true;
    }

    private static string SpotifyTrackToString(FullTrack? spotifyTrack)
    {
        if (spotifyTrack == null)
        {
            return "Не нашли трек в спотифае";
        }

        return $"Исполнители: {string.Join(" ", spotifyTrack.Artists.Select(artist => artist.Name))}\n" +
               $"Название трека: {spotifyTrack.Name}\n" +
               $"Альбом: {spotifyTrack.Album.Name}";
    }

    private static string YandexMusicTrackToString(YandexTrack? yandexTrack)
    {
        if (yandexTrack == null)
        {
            return "Не нашли трек в яндекс музыке";
        }

        return $"Исполнители: {string.Join(" ", yandexTrack.Artists.Select(artist => artist.Name))}\n" +
               $"Название трека: {yandexTrack.Title}\n" +
               $"Альбомы: {string.Join(" ", yandexTrack.Albums.Select(artist => artist.Title))}";
    }

    private async Task SendMessage(long chatId, string message)
    {
        await telegramBotClient.SendTextMessageAsync(
            chatId: chatId,
            text: message,
            cancellationToken: cancellationTokenSource.Token);
    }

    private readonly ITelegramBotClient telegramBotClient;
    private readonly ISpotifyService spotifyService;
    private readonly IYandexMusicService yandexMusicService;

    private readonly CancellationTokenSource cancellationTokenSource;
}