using Telegram.Bot;

namespace TelegramBot.WorkerService.Builder;

public interface ITelegramBotBuilder
{
    public ITelegramBotClient BuildClient();
}