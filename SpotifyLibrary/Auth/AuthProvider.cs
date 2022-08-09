﻿using Newtonsoft.Json;

namespace SpotifyLibrary.Auth;

public class AuthProvider : IAuthProvider
{
    public AuthData GetAuth()
    {
        var data = File.ReadAllText("Auth/spotifyAuth.json");
        return JsonConvert.DeserializeObject<AuthData>(data) ?? throw new Exception("Can't deserialize auth data");
    }
}