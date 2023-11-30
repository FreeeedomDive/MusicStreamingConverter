using Ninject;
using TelegramBot.PollingDaemon.Settings;
using TelemetryApp.Utilities.Extensions;

namespace TelegramBot.PollingDaemon.DI;

public class DependenciesConfigurator
{
    public StandardKernel BuildDependencies()
    {
        var appSettings = new SettingsProvider().GetSettings();
        var ninjectKernel = new StandardKernel()
            .ConfigureTelemetryClientWithLogger("MusicStreamingConverter", "TelegramBot", appSettings.TelemetryApiUrl)
            .WithApiClient(appSettings.MusicSearchApiUrl)
            .WithAuthProviders()
            .WithClientBuilders()
            .WithClients()
            .WithStringComparator()
            .WithLinksRecognizers()
            .WithExecutableServices();

        return ninjectKernel;
    }
}