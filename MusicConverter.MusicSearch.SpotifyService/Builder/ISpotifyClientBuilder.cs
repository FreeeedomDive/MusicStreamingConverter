using SpotifyAPI.Web;

namespace MusicConverter.MusicSearch.SpotifyService.Builder;

public interface ISpotifyClientBuilder
{
    ISpotifyClient BuildClient();
}