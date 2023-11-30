using Core.StringComparison;
using Core.StringComparison.Extensions;
using MusicSearch.Client;
using MusicSearch.Dto.Models;
using Telegram.Bot;
using TelegramBot.WorkerService.Extensions;

namespace TelegramBot.WorkerService.ResponseBuilders;

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
        var albumInfo = SpotifyAlbumToString(album);
        var searchInfoStrings = new List<string>();

        var query = $"{album.Artist?.Name} {album.Name}";
        var searchResults = await musicSearchClient.YandexMusic.FindAlbumsAsync(query);
        searchInfoStrings.Add(
            searchResults.Length.PluralizeString("результат", "результата", "результатов")
        );

        var sameYandexAlbum = searchResults
                              .Select(x => Convert(x, album))
                              .OrderByDescending(x => x.confidence)
                              .FirstOrDefault();
        var yandexTrackInfo = YandexMusicAlbumToString(sameYandexAlbum.album, sameYandexAlbum.confidence);

        await telegramBotClient.SendTextMessageAsync(
            chatId,
            $"{albumInfo}\n" +
            $"===========\n" +
            $"Результаты поиска\n" +
            $"{string.Join("\n", searchInfoStrings)}\n" +
            $"===========\n" +
            $"Альбом в Яндекс.Музыке\n" +
            $"{yandexTrackInfo}"
        );
        if (sameYandexAlbum.album is not null)
        {
            await telegramBotClient.SendTextMessageAsync(
                chatId,
                $"https://music.yandex.ru/album/{sameYandexAlbum.album.Id}"
            );
        }
    }

    private static string SpotifyAlbumToString(AlbumDto? spotifyAlbum, int? resultConfidence = null)
    {
        if (spotifyAlbum == null)
        {
            return "Не нашли альбом в спотифае";
        }

        return (resultConfidence == null
                   ? ""
                   : $"Уверенность в найденном результате: {resultConfidence}%\n")
               + $"Исполнитель: {string.Join(" ", spotifyAlbum.Artist?.Name)}\n"
               + $"Альбом: {spotifyAlbum.Name}";
    }

    private static string YandexMusicAlbumToString(AlbumDto? yandexAlbum, int? resultConfidence = null)
    {
        if (yandexAlbum == null)
        {
            return "Не нашли альбом в Яндекс.Музыке";
        }

        return (resultConfidence == null
                   ? ""
                   : $"Уверенность в найденном результате: {resultConfidence}%\n")
               + $"Исполнитель: {string.Join(" ", yandexAlbum.Artist?.Name)}\n"
               + $"Альбом: {yandexAlbum.Name}";
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