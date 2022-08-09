using SpotifyAPI.Web;

namespace SpotifyLibrary.Builder;

public interface ISpotifyClientBuilder
{
    ISpotifyClient BuildClient();
}