using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MusicSearch.Api.Middlewares;
using MusicSearch.Api.Options;
using MusicSearch.SpotifyService;
using SpotifyAPI.Web;
using TelemetryApp.Utilities.Extensions;
using TelemetryApp.Utilities.Middlewares;
using Yandex.Music.Api;
using Yandex.Music.Api.Common;
using YandexMusicLibrary;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<SpotifyOptions>(builder.Configuration.GetSection("Spotify"));

var telemetryApiUrl = builder.Configuration.GetSection("Telemetry").GetSection("ApiUrl").Value;
builder.Services.ConfigureTelemetryClientWithLogger("AntiClownBot", "Api", telemetryApiUrl);

builder.Services.AddSingleton<ISpotifyClient>(
    serviceProvider =>
    {
        var spotifyOptions = serviceProvider.GetRequiredService<IOptions<SpotifyOptions>>().Value;
        var config = SpotifyClientConfig
                     .CreateDefault()
                     .WithAuthenticator(new ClientCredentialsAuthenticator(spotifyOptions.ClientId, spotifyOptions.ClientSecret));

        return new SpotifyClient(config);
    }
);
builder.Services.AddTransient<AuthStorage>();
builder.Services.AddTransient<YandexMusicApi>();

builder.Services.AddTransient<ISpotifyService, SpotifyService>();
builder.Services.AddTransient<IYandexMusicService, YandexMusicService>();

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "StreamingMusic Converter Spotify API", Version = "v1" });
});

var app = builder.Build();

app.UseDeveloperExceptionPage();
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "StreamingMusic Converter Spotify API v1"));

app.UseHttpsRedirection();

app.UseRouting();
app.UseWebSockets();
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<ServiceExceptionHandlingMiddleware>();
app.UseEndpoints(endpoints => endpoints.MapControllers());
await app.RunAsync();