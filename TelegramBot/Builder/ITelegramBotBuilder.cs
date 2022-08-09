using Telegram.Bot;

namespace TelegramBot.Builder;

public interface ITelegramBotBuilder
{
    public ITelegramBotClient BuildClient();
}