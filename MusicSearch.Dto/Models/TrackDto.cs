namespace MusicSearch.Dto.Models;

public class TrackDto
{
    public string Id { get; set; }
    public string Title { get; set; }
    public ArtistDto? Artist { get; set; }
    public AlbumDto? Album { get; set; }
}