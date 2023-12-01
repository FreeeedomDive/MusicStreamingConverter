using MusicSearch.Dto.Models;

namespace TelegramBot.WorkerService.ResponseBuilders;

public static class ResourceToStringBuilder
{
    public static string SpotifyTrackToString(TrackDto? spotifyTrack, int? resultConfidence = null)
    {
        if (spotifyTrack == null)
        {
            return "Трек не найден в Spotify";
        }

        return (resultConfidence == null
                   ? ""
                   : $"Уверенность в найденном результате: {resultConfidence}%\n")
               + $"Исполнитель: {string.Join(" ", spotifyTrack.Artist?.Name)}\n"
               + $"Название трека: {spotifyTrack.Title}\n"
               + $"Альбом: {spotifyTrack.Album?.Name}";
    }

    public static string YandexMusicTrackToString(TrackDto? yandexTrack, int? resultConfidence = null)
    {
        if (yandexTrack == null)
        {
            return "Трек не найден в Яндекс.Музыке";
        }

        return (resultConfidence == null
                   ? ""
                   : $"Уверенность в найденном результате: {resultConfidence}%\n")
               + $"Исполнитель: {string.Join(" ", yandexTrack.Artist?.Name)}\n"
               + $"Название трека: {yandexTrack.Title}\n"
               + $"Альбом: {string.Join(" ", yandexTrack.Album?.Name)}";
    }

    public static string SpotifyAlbumToString(AlbumDto? spotifyAlbum, int? resultConfidence = null)
    {
        if (spotifyAlbum == null)
        {
            return "Альбом не найден в Spotify";
        }

        return (resultConfidence == null
                   ? ""
                   : $"Уверенность в найденном результате: {resultConfidence}%\n")
               + $"Исполнитель: {string.Join(" ", spotifyAlbum.Artist?.Name)}\n"
               + $"Альбом: {spotifyAlbum.Name}";
    }

    public static string YandexMusicAlbumToString(AlbumDto? yandexAlbum, int? resultConfidence = null)
    {
        if (yandexAlbum == null)
        {
            return "Альбом не найден в Яндекс.Музыке";
        }

        return (resultConfidence == null
                   ? ""
                   : $"Уверенность в найденном результате: {resultConfidence}%\n")
               + $"Исполнитель: {string.Join(" ", yandexAlbum.Artist?.Name)}\n"
               + $"Альбом: {yandexAlbum.Name}";
    }
}