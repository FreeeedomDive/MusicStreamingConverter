namespace TelegramBot.Auth;

public interface IAuthProvider
{
    public AuthData GetAuth();
}