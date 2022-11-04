using Loggers;
using MusicSearch.Client;
using Ninject;
using RestClient;
using Telegram.Bot;
using TelegramBot.WorkerService;
using TelegramBot.WorkerService.Builder;
using ILogger = Loggers.ILogger;

namespace TelegramBot.PollingDaemon.DI;

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
        standardKernel.Bind<WorkerService.Auth.IAuthProvider>().To<WorkerService.Auth.AuthProvider>();

        return standardKernel;
    }

    public static StandardKernel WithClientBuilders(this StandardKernel standardKernel)
    {
        standardKernel.Bind<ITelegramBotBuilder>().To<TelegramBotBuilder>();

        return standardKernel;
    }

    public static StandardKernel WithApiClient(this StandardKernel standardKernel)
    {
        var restClient = RestClientBuilder.BuildRestClient("https://localhost:3280", false);
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