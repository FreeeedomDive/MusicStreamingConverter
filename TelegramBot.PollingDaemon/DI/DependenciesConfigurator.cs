using Ninject;
using TelemetryApp.Utilities.Extensions;

namespace TelegramBot.PollingDaemon.DI;

public class DependenciesConfigurator
{
    public StandardKernel BuildDependencies()
    {
        var ninjectKernel = new StandardKernel()
            .ConfigureLoggerClient("MusicStreamingConverter", "TelegramBot")
            .ConfigureApiTelemetryClient("MusicStreamingConverter", "TelegramBot")
            .WithApiClient()
            .WithAuthProviders()
            .WithClientBuilders()
            .WithClients()
            .WithStringComparator()
            .WithExecutableServices();

        return ninjectKernel;
    }
}