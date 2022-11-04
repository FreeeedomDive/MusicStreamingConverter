using Newtonsoft.Json;

namespace YandexMusicLibrary.Auth;

public class AuthProvider : IAuthProvider
{
    public AuthData GetAuth()
    {
        var data = File.ReadAllText("../Files/Auth/yandexAuth.json");
        return JsonConvert.DeserializeObject<AuthData>(data) ?? throw new Exception("Can't deserialize auth data");
    }
}