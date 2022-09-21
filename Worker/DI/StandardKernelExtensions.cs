using Loggers;
using Ninject;
using NLog;
using SpotifyAPI.Web;
using SpotifyLibrary;
using SpotifyLibrary.Builder;
using Telegram.Bot;
using TelegramBot;
using TelegramBot.Builder;
using Yandex.Music.Api;
using YandexMusicLibrary;
using YandexMusicLibrary.Builder;
using ILogger = Loggers.ILogger;

namespace Worker.DI;

public static class StandardKernelExtensions
{
    public static StandardKernel WithLogger(this StandardKernel ninjectKernel)
    {
        var logger = LogManager.GetLogger("Default");
        ninjectKernel.Bind<Logger>().ToConstant(logger);
        ninjectKernel.Bind<ILogger>().To<NLogger>();

        return ninjectKernel;
    }

    public static StandardKernel WithAuthProviders(this StandardKernel standardKernel)
    {
        standardKernel.Bind<SpotifyLibrary.Auth.IAuthProvider>().To<SpotifyLibrary.Auth.AuthProvider>();
        standardKernel.Bind<YandexMusicLibrary.Auth.IAuthProvider>().To<YandexMusicLibrary.Auth.AuthProvider>();
        standardKernel.Bind<TelegramBot.Auth.IAuthProvider>().To<TelegramBot.Auth.AuthProvider>();

        return standardKernel;
    }

    public static StandardKernel WithClientBuilders(this StandardKernel standardKernel)
    {
        standardKernel.Bind<ISpotifyClientBuilder>().To<SpotifyClientBuilder>();
        standardKernel.Bind<IYandexMusicBuilder>().To<YandexMusicBuilder>();
        standardKernel.Bind<ITelegramBotBuilder>().To<TelegramBotBuilder>();

        return standardKernel;
    }

    public static StandardKernel WithClients(this StandardKernel standardKernel)
    {
        var spotifyClient = standardKernel.Get<ISpotifyClientBuilder>().BuildClient();
        standardKernel.Bind<ISpotifyClient>().ToConstant(spotifyClient);

        var yandexClient = standardKernel.Get<IYandexMusicBuilder>().BuildClient();
        standardKernel.Bind<YandexApi>().ToConstant(yandexClient);

        var telegramBot = standardKernel.Get<ITelegramBotBuilder>().BuildClient();
        standardKernel.Bind<ITelegramBotClient>().ToConstant(telegramBot);

        return standardKernel;
    }

    public static StandardKernel WithExecutableServices(this StandardKernel standardKernel)
    {
        standardKernel.Bind<ISpotifyService>().To<SpotifyService>();
        standardKernel.Bind<IYandexMusicService>().To<YandexMusicService>();
        standardKernel.Bind<ITelegramWorker>().To<TelegramWorker>();

        return standardKernel;
    }
}