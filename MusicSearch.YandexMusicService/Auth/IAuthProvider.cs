namespace YandexMusicLibrary.Auth;

public interface IAuthProvider
{
    public AuthData GetAuth();
}