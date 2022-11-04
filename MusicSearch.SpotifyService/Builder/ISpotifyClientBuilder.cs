using SpotifyAPI.Web;

namespace MusicSearch.SpotifyService.Builder;

public interface ISpotifyClientBuilder
{
    ISpotifyClient BuildClient();
}