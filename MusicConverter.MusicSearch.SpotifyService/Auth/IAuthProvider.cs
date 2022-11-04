namespace MusicConverter.MusicSearch.SpotifyService.Auth;

public interface IAuthProvider
{
    public AuthData GetAuth();
}