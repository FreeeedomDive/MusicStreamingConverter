using System.Text.RegularExpressions;

namespace Core.LinksRecognizers.Yandex;

public partial class YandexLinksRecognizeService : IYandexLinksRecognizeService
{
    public ResourceLink? TryRecognize(string link)
    {
        var trackId = TryRecognizeAsTrack(link);
        if (trackId is not null)
        {
            return new ResourceLink
            {
                Type = LinkType.Track,
                Id = trackId,
            };
        }

        var albumId = TryRecognizeAsAlbum(link);
        if (albumId is not null)
        {
            return new ResourceLink
            {
                Type = LinkType.Album,
                Id = albumId,
            };
        }

        var artistId = TryRecognizeAsArtist(link);
        if (artistId is not null)
        {
            return new ResourceLink
            {
                Type = LinkType.Artist,
                Id = artistId,
            };
        }

        return null;
    }

    private string? TryRecognizeAsTrack(string link)
    {
        var regex = YandexMusicTrackLinkRegex();
        var result = regex.Match(link);

        return result.Success ? result.Groups[2].Captures[0].Value : null;
    }

    private string? TryRecognizeAsAlbum(string link)
    {
        var regex = YandexMusicAlbumLinkRegex();
        var result = regex.Match(link);

        return result.Success ? result.Groups[2].Captures[0].Value : null;
    }

    private string? TryRecognizeAsArtist(string link)
    {
        var regex = YandexMusicArtistLinkRegex();
        var result = regex.Match(link);

        return result.Success ? result.Groups[2].Captures[0].Value : null;
    }

    [GeneratedRegex("https://music.yandex.(ru|com)/album/.*/track/(\\d*)(.*)")]
    private static partial Regex YandexMusicTrackLinkRegex();

    [GeneratedRegex("https://music.yandex.(ru|com)/album/(\\d*)(.*)")]
    private static partial Regex YandexMusicAlbumLinkRegex();

    [GeneratedRegex("https://music.yandex.(ru|com)/artist/(\\d*)(.*)")]
    private static partial Regex YandexMusicArtistLinkRegex();
}