using Microsoft.OpenApi.Models;
using MusicConverter.MusicSearch.Api.Middlewares;
using MusicConverter.MusicSearch.SpotifyService;
using MusicConverter.MusicSearch.SpotifyService.Auth;
using MusicConverter.MusicSearch.SpotifyService.Builder;
using YandexMusicLibrary;
using YandexMusicLibrary.Builder;

namespace MusicConverter.MusicSearch.Api;

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
        services.AddSingleton(yandexClientBuilder.BuildClient());
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
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}