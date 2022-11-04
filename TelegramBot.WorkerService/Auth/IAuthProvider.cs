namespace TelegramBot.WorkerService.Auth;

public interface IAuthProvider
{
    public AuthData GetAuth();
}