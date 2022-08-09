using Newtonsoft.Json;

namespace TelegramBot.Auth;

public class AuthProvider : IAuthProvider
{
    public AuthData GetAuth()
    {
        var data = File.ReadAllText("Auth/telegramAuth.json");
        return JsonConvert.DeserializeObject<AuthData>(data) ?? throw new Exception("Can't deserialize auth data");
    }
}