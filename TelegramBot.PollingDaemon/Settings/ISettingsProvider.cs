namespace TelegramBot.PollingDaemon.Settings;

public interface ISettingsProvider
{
    AppSettings GetSettings();
}