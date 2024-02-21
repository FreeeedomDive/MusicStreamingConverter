namespace Core.LinksRecognizers.Yandex;

public interface IYandexLinksRecognizeService
{
    ResourceLink? TryRecognize(string link);
}