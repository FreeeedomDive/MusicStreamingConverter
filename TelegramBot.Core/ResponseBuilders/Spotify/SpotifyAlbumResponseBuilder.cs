using System.Text;
using Core.StringComparison;
using Core.StringComparison.Extensions;
using MusicSearch.Client;
using MusicSearch.Dto.Models;
using Telegram.Bot;
using TelegramBot.Core.Extensions;

namespace TelegramBot.Core.ResponseBuilders.Spotify;

public class SpotifyAlbumResponseBuilder
(
    IMusicSearchClient musicSearchClient,
    ITelegramBotClient telegramBotClient,
    IStringComparison stringComparison
) : ISpotifyAlbumResponseBuilder
{
    public async Task BuildAsync(long chatId, string albumId)
    {
        var album = await musicSearchClient.Spotify.GetAlbumAsync(albumId);
        var albumInfo = ResourceToStringBuilder.SpotifyAlbumToString(album);

        var query = $"{album.Artist?.Name} {album.Name}";
        var searchResults = await musicSearchClient.YandexMusic.FindAlbumsAsync(query);

        var sameYandexAlbum = searchResults
                              .Select(x => Convert(x, album))
                              .OrderByDescending(x => x.confidence)
                              .FirstOrDefault();
        var yandexAlbumInfo = ResourceToStringBuilder.YandexMusicAlbumToString(sameYandexAlbum.album, sameYandexAlbum.confidence);

        await telegramBotClient.SendTextMessageAsync(
            chatId,
            new StringBuilder()
                .AppendLine(albumInfo)
                .AppendLine("===========")
                .AppendLine("Результаты поиска")
                .AppendLine(searchResults.Length.PluralizeString("результат", "результата", "результатов"))
                .AppendLine("===========")
                .AppendLine("Альбом в Яндекс.Музыке")
                .Append(yandexAlbumInfo)
                .ToString()
        );
        if (sameYandexAlbum.album is not null)
        {
            await telegramBotClient.SendTextMessageAsync(
                chatId,
                $"https://music.yandex.ru/album/{sameYandexAlbum.album.Id}"
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