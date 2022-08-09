namespace SpotifyLibrary.Auth;

public interface IAuthProvider
{
    public AuthData GetAuth();
}