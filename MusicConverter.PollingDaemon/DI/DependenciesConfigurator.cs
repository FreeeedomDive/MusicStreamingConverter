using Ninject;

namespace MusicConverter.PollingDaemon.DI;

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
            .WithExecutableServices();

        return ninjectKernel;
    }
}