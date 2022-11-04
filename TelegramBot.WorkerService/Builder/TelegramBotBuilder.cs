using Telegram.Bot;
using TelegramBot.WorkerService.Auth;

namespace TelegramBot.WorkerService.Builder;

public class TelegramBotBuilder : ITelegramBotBuilder
{
    public TelegramBotBuilder(IAuthProvider authProvider)
    {
        this.authProvider = authProvider;
    }
    
    public ITelegramBotClient BuildClient()
    {
        var auth = authProvider.GetAuth();
        return new TelegramBotClient(auth.TelegramBotToken);
    }

    private readonly IAuthProvider authProvider;
}