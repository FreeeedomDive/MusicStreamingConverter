﻿namespace MusicSearch.Dto.Models;

public class ArtistDto
{
    public string Id { get; set; }
    public string Name { get; set; }

    /// <summary>
    ///     Заполняется только для треков с Source = Spotify
    /// </summary>
    public string Uri { get; set; }
}