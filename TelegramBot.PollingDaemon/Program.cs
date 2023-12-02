using Core.LinksRecognizers.Spotify;
using Core.LinksRecognizers.Yandex;
using Core.RestClient;
using Core.StringComparison;
using Microsoft.Extensions.Options;
using MusicSearch.Client;
using Telegram.Bot;
using TelegramBot.PollingDaemon;
using TelegramBot.PollingDaemon.Options;
using TelegramBot.Core;
using TelegramBot.Core.ResponseBuilders;
using TelegramBot.Core.ResponseBuilders.Spotify;
using TelegramBot.Core.ResponseBuilders.YandexMusic;
using TelegramBot.Core.Services.Compare;
using TelegramBot.Core.Services.Match;
using TelegramBot.Core.Services.Search;
using TelemetryApp.Utilities.Extensions;

var builder = WebApplication.CreateBuilder(args);

var telemetryApiUrl = builder.Configuration.GetSection("Telemetry").GetSection("ApiUrl").Value;
builder.Services.ConfigureTelemetryClientWithLogger("MusicStreamingConverter", "TelegramBot", telemetryApiUrl);

builder.Services.Configure<MusicSearchOptions>(builder.Configuration.GetRequiredSection("MusicSearch"));
builder.Services.Configure<TelegramOptions>(builder.Configuration.GetRequiredSection("Telegram"));

builder.Services.AddSingleton<IMusicSearchClient>(
    serviceProvider =>
    {
        var apiUrl = serviceProvider.GetRequiredService<IOptions<MusicSearchOptions>>().Value.ApiUrl;
        var restClient = RestClientBuilder.BuildRestClient(apiUrl, false);

        return new MusicSearchClient(restClient);
    }
);

builder.Services.AddSingleton<ITelegramBotClient>(
    serviceProvider =>
    {
        var botToken = serviceProvider.GetRequiredService<IOptions<TelegramOptions>>().Value.BotToken;
        return new TelegramBotClient(botToken);
    }
);

builder.Services.AddTransient<ISpotifyLinksRecognizeService, SpotifyLinksRecognizeService>();
builder.Services.AddTransient<IYandexLinksRecognizeService, YandexLinksRecognizeService>();

builder.Services.AddTransient<IResourceSearcher, ResourceSearcher>();
builder.Services.AddTransient<IResourceComparer, ResourceComparer>();
builder.Services.AddTransient<IResourceMatcher, ResourceMatcher>();

builder.Services.AddTransient<ISpotifyTrackResponseBuilder, SpotifyTrackResponseBuilder>();
builder.Services.AddTransient<IYandexMusicTrackResponseBuilder, YandexMusicTrackResponseBuilder>();
builder.Services.AddTransient<ISpotifyAlbumResponseBuilder, SpotifyAlbumResponseBuilder>();
builder.Services.AddTransient<IYandexMusicAlbumResponseBuilder, YandexMusicAlbumResponseBuilder>();
builder.Services.AddTransient<ISpotifyArtistResponseBuilder, SpotifyArtistResponseBuilder>();
builder.Services.AddTransient<IYandexMusicArtistResponseBuilder, YandexMusicArtistResponseBuilder>();

builder.Services.AddTransient<IStringComparison, LevenshteinDistanceStringComparison>();
builder.Services.AddSingleton<ITelegramWorker, TelegramWorker>();

var app = builder.Build();

var telegramWorker = app.Services.GetRequiredService<ITelegramWorker>();
await telegramWorker.StartAsync();
