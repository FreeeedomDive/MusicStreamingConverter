using MusicConverter.PollingDaemon.DI;
using Ninject;
using TelegramBot;

namespace MusicConverter.PollingDaemon;

public class EntryPoint
{
    public static async Task Main()
    {
        var kernel = new DependenciesConfigurator().BuildDependencies();
        var telegramWorker = kernel.Get<ITelegramWorker>();

        await telegramWorker.Start();
    }
}