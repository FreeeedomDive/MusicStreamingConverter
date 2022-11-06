using Microsoft.OpenApi.Models;
using MusicSearch.SpotifyService;
using MusicSearch.SpotifyService.Auth;
using MusicSearch.SpotifyService.Builder;
using MusicSearch.Api.Middlewares;
using YandexMusicLibrary;
using YandexMusicLibrary.Builder;

namespace MusicSearch.Api;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        var spotifyClientAuthProvider = new AuthProvider();
        var spotifyClientBuilder = new SpotifyClientBuilder(spotifyClientAuthProvider);
        services.AddSingleton(spotifyClientBuilder.BuildClient());
        services.AddTransient<ISpotifyService, SpotifyService.SpotifyService>();

        var yandexClientAuthProvider = new YandexMusicLibrary.Auth.AuthProvider();
        var yandexClientBuilder = new YandexMusicBuilder(yandexClientAuthProvider);
        var authStorage = yandexClientBuilder.BuildAuthStorage();
        services.AddSingleton(authStorage);
        services.AddSingleton(yandexClientBuilder.BuildClient(authStorage).GetAwaiter().GetResult());
        services.AddTransient<IYandexMusicService, YandexMusicService>();
        
        services.AddControllers();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "StreamingMusic Converter Spotify API", Version = "v1" });
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "StreamingMusic Converter Spotify API v1"));
        }

        app.UseHttpsRedirection();

        app.UseRouting();
        app.UseWebSockets();
        app.UseMiddleware<RequestLoggingMiddleware>();
        app.UseMiddleware<ServiceExceptionHandlingMiddleware>();
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}