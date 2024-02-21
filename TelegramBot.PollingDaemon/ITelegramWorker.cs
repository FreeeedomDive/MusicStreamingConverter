namespace TelegramBot.PollingDaemon;

public interface ITelegramWorker
{
    public Task StartAsync();
}