namespace MusicSearch.Dto.Models;

public class AlbumDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public ArtistDto? Artist { get; set; }
}