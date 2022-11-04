using Loggers;
using MusicConverter.MusicSearch.Client;
using Ninject;
using NLog;
using SpotifyAPI.Web;
using MusicConverter.MusicSearch.SpotifyService;
using MusicConverter.MusicSearch.SpotifyService.Builder;
using RestClient;
using Telegram.Bot;
using TelegramBot;
using TelegramBot.Builder;
using Yandex.Music.Api;
using YandexMusicLibrary;
using YandexMusicLibrary.Builder;
using ILogger = Loggers.ILogger;

namespace MusicConverter.PollingDaemon.DI;

public static class StandardKernelExtensions
{
    public static StandardKernel WithLogger(this StandardKernel ninjectKernel)
    {
        var logger = NLogger.Build("Default");
        ninjectKernel.Bind<ILogger>().ToConstant(logger);

        return ninjectKernel;
    }

    public static StandardKernel WithAuthProviders(this StandardKernel standardKernel)
    {
        standardKernel.Bind<TelegramBot.Auth.IAuthProvider>().To<TelegramBot.Auth.AuthProvider>();

        return standardKernel;
    }

    public static StandardKernel WithClientBuilders(this StandardKernel standardKernel)
    {
        standardKernel.Bind<ITelegramBotBuilder>().To<TelegramBotBuilder>();

        return standardKernel;
    }

    public static StandardKernel WithApiClient(this StandardKernel standardKernel)
    {
        var restClient = RestClientBuilder.BuildRestClient("https://localhost:3280");
        standardKernel.Bind<RestSharp.RestClient>().ToConstant(restClient);
        standardKernel.Bind<IMusicSearchClient>().To<MusicSearchClient>();

        return standardKernel;
    }

    public static StandardKernel WithClients(this StandardKernel standardKernel)
    {
        var telegramBot = standardKernel.Get<ITelegramBotBuilder>().BuildClient();
        standardKernel.Bind<ITelegramBotClient>().ToConstant(telegramBot);

        return standardKernel;
    }

    public static StandardKernel WithExecutableServices(this StandardKernel standardKernel)
    {
        standardKernel.Bind<ITelegramWorker>().To<TelegramWorker>();

        return standardKernel;
    }
}