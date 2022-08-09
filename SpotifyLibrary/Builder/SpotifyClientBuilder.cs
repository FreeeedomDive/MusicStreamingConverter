using SpotifyAPI.Web;
using SpotifyLibrary.Auth;

namespace SpotifyLibrary.Builder;

public class SpotifyClientBuilder : ISpotifyClientBuilder
{
    public SpotifyClientBuilder(IAuthProvider authProvider)
    {
        this.authProvider = authProvider;
    }
    
    public ISpotifyClient BuildClient()
    {
        var auth = authProvider.GetAuth();
        var config = SpotifyClientConfig
            .CreateDefault()
            .WithAuthenticator(new ClientCredentialsAuthenticator(auth.SpotifyClientId, auth.SpotifyClientSecret));

        return new SpotifyClient(config);
    }

    private readonly IAuthProvider authProvider;
}