﻿using SpotifyAPI.Web;

namespace MusicSearch.Client.SpotifyClient;

public interface ISpotifyClient
{
    public Task<FullTrack[]> FindTracksAsync(string query, int skip = 0, int take = 10);
    public Task<FullArtist[]> FindArtistsAsync(string query, int skip = 0, int take = 10);
    public Task<SimpleAlbum[]> FindAlbumsAsync(string query, int skip = 0, int take = 10);
    
    public Task<FullTrack> GetTrackAsync(string id);
    public Task<FullArtist> GetArtistAsync(string id);
    public Task<FullAlbum> GetAlbumAsync(string id);
}