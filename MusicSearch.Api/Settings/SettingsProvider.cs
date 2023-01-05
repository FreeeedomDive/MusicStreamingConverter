using Newtonsoft.Json;

namespace MusicSearch.Api.Settings;

public class SettingsProvider : ISettingsProvider
{
    public AppSettings GetSettings()
    {
        var data = File.ReadAllText("Settings/settings.json");
        return JsonConvert.DeserializeObject<AppSettings>(data) ?? throw new Exception("Can't deserialize settings");
    }
}