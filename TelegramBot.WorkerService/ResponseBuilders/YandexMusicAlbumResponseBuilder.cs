using System.Text;
using Core.StringComparison;
using Core.StringComparison.Extensions;
using MusicSearch.Client;
using MusicSearch.Dto.Models;
using Telegram.Bot;
using TelegramBot.WorkerService.Extensions;

namespace TelegramBot.WorkerService.ResponseBuilders;

public class YandexMusicAlbumResponseBuilder
(
    IMusicSearchClient musicSearchClient,
    ITelegramBotClient telegramBotClient,
    IStringComparison stringComparison
) : IYandexMusicAlbumResponseBuilder
{
    public async Task BuildAsync(long chatId, string albumId)
    {
        var album = await musicSearchClient.YandexMusic.GetAlbumAsync(albumId);
        var albumInfo = ResourceToStringBuilder.YandexMusicAlbumToString(album);

        var query = $"{album.Artist?.Name} {album.Name}";
        var searchResults = await musicSearchClient.Spotify.FindAlbumsAsync(query);

        var sameSpotifyAlbum = searchResults
                               .Select(x => Convert(x, album))
                               .OrderByDescending(x => x.confidence)
                               .FirstOrDefault();
        var spotifyAlbumInfo = ResourceToStringBuilder.SpotifyAlbumToString(sameSpotifyAlbum.album, sameSpotifyAlbum.confidence);

        await telegramBotClient.SendTextMessageAsync(
            chatId,
            new StringBuilder()
                .AppendLine(albumInfo)
                .AppendLine("===========")
                .AppendLine("Результаты поиска")
                .AppendLine(searchResults.Length.PluralizeString("результат", "результата", "результатов"))
                .AppendLine("===========")
                .AppendLine("Альбом в Spotify")
                .Append(spotifyAlbumInfo)
                .ToString()
        );
        if (sameSpotifyAlbum.album is not null)
        {
            await telegramBotClient.SendTextMessageAsync(
                chatId,
                $"https://open.spotify.com/album/{sameSpotifyAlbum.album.Uri["spotify:album:".Length..]}"
            );
        }
    }

    private (AlbumDto? album, int confidence) Convert(AlbumDto? albumDto, AlbumDto original)
    {
        if (albumDto is null)
        {
            return (null, 0);
        }

        var confidence = stringComparison.CompareAlbums(original, albumDto);
        return (albumDto, confidence);
    }
}