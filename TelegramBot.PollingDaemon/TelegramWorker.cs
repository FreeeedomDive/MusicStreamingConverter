using Core.LinksRecognizers;
using Core.LinksRecognizers.Spotify;
using Core.LinksRecognizers.Yandex;
using MusicSearch.Dto.Exceptions;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot.Core.ResponseBuilders;
using TelegramBot.Core.ResponseBuilders.Spotify;
using TelegramBot.Core.ResponseBuilders.YandexMusic;
using TelegramBot.Core.Services.Match;
using TelemetryApp.Api.Client.Log;

namespace TelegramBot.PollingDaemon;

public class TelegramWorker
(
    ITelegramBotClient telegramBotClient,
    ISpotifyLinksRecognizeService spotifyLinksRecognizeService,
    IYandexLinksRecognizeService yandexLinksRecognizeService,
    ISpotifyTrackResponseBuilder spotifyTrackResponseBuilder,
    IYandexMusicTrackResponseBuilder yandexMusicTrackResponseBuilder,
    ISpotifyAlbumResponseBuilder spotifyAlbumResponseBuilder,
    IYandexMusicAlbumResponseBuilder yandexMusicAlbumResponseBuilder,
    ISpotifyArtistResponseBuilder spotifyArtistResponseBuilder,
    IYandexMusicArtistResponseBuilder yandexMusicArtistResponseBuilder,
    ILoggerClient logger
) : ITelegramWorker
{
    public async Task StartAsync()
    {
        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = Array.Empty<UpdateType>(),
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
        {
            return;
        }

        var chatId = message.Chat.Id;
        try
        {
            if (messageText == "/start")
            {
                await SendMessage(
                    chatId,
                    "Этот бот позволяет искать исполнителей, альбомы и треки в стриминговых сервисах. " +
                    "Например, получив ссылку на трек в Спотифае, я попытаюсь его найти в Яндекс.Музыке и наоборот. " +
                    "Поддерживаются только эти два сервиса."
                );
                return;
            }

            var spotifyResource = await spotifyLinksRecognizeService.TryRecognizeAsync(messageText);
            if (spotifyResource is not null)
            {
                await HandleSpotifyLinkAsync(chatId, spotifyResource);
                await logger.InfoAsync(
                    "Successful convert {Resource} from Spotify\n{Username}: {Message}",
                    spotifyResource.Type, message.Chat.Username ?? chatId.ToString(), messageText
                );
                return;
            }

            var yandexResource = yandexLinksRecognizeService.TryRecognize(messageText);
            if (yandexResource is not null)
            {
                await HandleYandexMusicLinkAsync(chatId, yandexResource);
                await logger.InfoAsync(
                    "Successful convert {Resource} from Spotify\n{Username}: {Message}",
                    yandexResource.Type, message.Chat.Username ?? chatId.ToString(), messageText
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
                await SendMessage(
                    chatId,
                    "Возникла ошибка при обработке\nСкорее всего яндекс просит ввести капчу, так что нужно подождать"
                );
                return;
            }

            await SendMessage(chatId, $"Возникла ошибка при обработке\n{e.Message}");
        }
    }

    private async Task HandleSpotifyLinkAsync(long chatId, ResourceLink link)
    {
        var result = link.Type switch
        {
            LinkType.Track => await spotifyTrackResponseBuilder.BuildAsync(link.Id),
            LinkType.Album => await spotifyAlbumResponseBuilder.BuildAsync(link.Id),
            LinkType.Artist => await spotifyArtistResponseBuilder.BuildAsync(link.Id),
            _ => throw new ArgumentOutOfRangeException(nameof(link)),
        };
        await SendMessage(chatId, result);
    }

    private async Task HandleYandexMusicLinkAsync(long chatId, ResourceLink link)
    {
        var result = link.Type switch
        {
            LinkType.Track => await yandexMusicTrackResponseBuilder.BuildAsync(link.Id),
            LinkType.Album => await yandexMusicAlbumResponseBuilder.BuildAsync(link.Id),
            LinkType.Artist => await yandexMusicArtistResponseBuilder.BuildAsync(link.Id),
            _ => throw new ArgumentOutOfRangeException(nameof(link)),
        };
        await SendMessage(chatId, result);
    }

    private async Task SendMessage(long chatId, string message)
    {
        await telegramBotClient.SendTextMessageAsync(
            chatId: chatId,
            text: message,
            cancellationToken: cancellationTokenSource.Token
        );
    }

    private readonly CancellationTokenSource cancellationTokenSource = new();
}