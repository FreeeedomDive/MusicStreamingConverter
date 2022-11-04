using Ninject;
using TelegramBot.PollingDaemon.DI;
using TelegramBot.WorkerService;

namespace TelegramBot.PollingDaemon;

public class EntryPoint
{
    public static async Task Main()
    {
        var kernel = new DependenciesConfigurator().BuildDependencies();
        var telegramWorker = kernel.Get<ITelegramWorker>();

        await telegramWorker.Start();
    }
}