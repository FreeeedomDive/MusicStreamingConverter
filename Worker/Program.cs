using Ninject;
using TelegramBot;
using Worker.DI;

namespace Worker;

public class EntryPoint
{
    public static async Task Main()
    {
        var kernel = new DependenciesConfigurator().BuildDependencies();
        var telegramWorker = kernel.Get<ITelegramWorker>();

        await telegramWorker.Start();
    }
}