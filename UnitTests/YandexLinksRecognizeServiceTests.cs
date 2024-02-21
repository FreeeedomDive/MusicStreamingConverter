using Core.LinksRecognizers;
using Core.LinksRecognizers.Yandex;
using FluentAssertions;

namespace UnitTests;

public class YandexLinksRecognizeServiceTests
{
    [SetUp]
    public void Setup()
    {
        yandexLinksRecognizeService = new YandexLinksRecognizeService();
    }

    // tracks
    [TestCase("https://music.yandex.ru/album/21940/track/178529", LinkType.Track, "178529")]
    [TestCase("https://music.yandex.ru/album/154525/track/170252", LinkType.Track, "170252")]
    [TestCase("https://music.yandex.ru/album/11139074/track/57897457", LinkType.Track, "57897457")]
    [TestCase("https://music.yandex.ru/album/27019045/track/4395339", LinkType.Track, "4395339")]
    [TestCase("https://music.yandex.ru/album/18111/track/178504", LinkType.Track, "178504")]
    // albums
    [TestCase("https://music.yandex.ru/album/21940", LinkType.Album, "21940")]
    [TestCase("https://music.yandex.ru/album/154525", LinkType.Album, "154525")]
    [TestCase("https://music.yandex.ru/album/11139074", LinkType.Album, "11139074")]
    [TestCase("https://music.yandex.ru/album/27019045", LinkType.Album, "27019045")]
    [TestCase("https://music.yandex.ru/album/18111", LinkType.Album, "18111")]
    // artists
    [TestCase("https://music.yandex.ru/artist/36800", LinkType.Artist, "36800")]
    [TestCase("https://music.yandex.ru/artist/386878", LinkType.Artist, "386878")]
    [TestCase("https://music.yandex.ru/artist/12674", LinkType.Artist, "12674")]
    [TestCase("https://music.yandex.ru/artist/5663008", LinkType.Artist, "5663008")]
    [TestCase("https://music.yandex.ru/artist/930335", LinkType.Artist, "930335")]
    public void TestRecognizer(string link, LinkType expectedLinkType, string expectedId)
    {
        var resourceLink = yandexLinksRecognizeService.TryRecognize(link);
        resourceLink.Should().NotBeNull();
        resourceLink!.Type.Should().Be(expectedLinkType);
        resourceLink.Id.Should().BeEquivalentTo(expectedId);
    }

    private IYandexLinksRecognizeService yandexLinksRecognizeService = null!;
}