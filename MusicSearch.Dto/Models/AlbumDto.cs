namespace MusicSearch.Dto.Models;

public class AlbumDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public ArtistDto? Artist { get; set; }
    public Source Source { get; set; }

    /// <summary>
    ///     Заполняется только для треков с Source = Spotify
    /// </summary>
    public string Uri { get; set; }
}