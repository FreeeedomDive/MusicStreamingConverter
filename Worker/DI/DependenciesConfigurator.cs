using Ninject;

namespace Worker.DI;

public class DependenciesConfigurator
{
    public StandardKernel BuildDependencies()
    {
        var ninjectKernel = new StandardKernel()
            .WithLogger()
            .WithAuthProviders()
            .WithClientBuilders()
            .WithClients()
            .WithExecutableServices();

        return ninjectKernel;
    }
}