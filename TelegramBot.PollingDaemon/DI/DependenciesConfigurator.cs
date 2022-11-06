using Ninject;

namespace TelegramBot.PollingDaemon.DI;

public class DependenciesConfigurator
{
    public StandardKernel BuildDependencies()
    {
        var ninjectKernel = new StandardKernel()
            .WithLogger()
            .WithApiClient()
            .WithAuthProviders()
            .WithClientBuilders()
            .WithClients()
            .WithStringComparator()
            .WithExecutableServices();

        return ninjectKernel;
    }
}