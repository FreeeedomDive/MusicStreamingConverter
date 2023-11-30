using Core.LinksRecognizers;
using Core.LinksRecognizers.Spotify;
using FluentAssertions;

namespace UnitTests;

public class SpotifyLinksRecognizeServiceTests
{
    [SetUp]
    public void Setup()
    {
        spotifyLinksRecognizeService = new SpotifyLinksRecognizeService();
    }

    // tracks, old format
    [TestCase("https://open.spotify.com/track/2dARzHBK7Srnu3zayBNxl9?si=6a97dbd53b9b41a7", LinkType.Track, "2dARzHBK7Srnu3zayBNxl9")]
    [TestCase("https://open.spotify.com/track/1cBxAm8a0fENn2ix3Dfm3u", LinkType.Track, "1cBxAm8a0fENn2ix3Dfm3u")]
    [TestCase("https://open.spotify.com/track/2cTOee6bTauDFK1QuDS1BD?si=4790c30308eb47e7", LinkType.Track, "2cTOee6bTauDFK1QuDS1BD")]
    [TestCase("https://open.spotify.com/track/6WTioFyIEiYB5Ge0SJO8Rd", LinkType.Track, "6WTioFyIEiYB5Ge0SJO8Rd")]
    [TestCase("https://open.spotify.com/track/5hheGdf1cb4rK0FNiedCfK?si=80d7de24dbe54da3", LinkType.Track, "5hheGdf1cb4rK0FNiedCfK")]
    // albums, old format
    [TestCase("https://open.spotify.com/album/1Rty2CL0UZeJWeCcCiXmhg", LinkType.Album, "1Rty2CL0UZeJWeCcCiXmhg")]
    [TestCase("https://open.spotify.com/album/1YzifcxmauqYL9BXr0rkKs?si=_ygv1aflTASf5iLbcG1RFw", LinkType.Album, "1YzifcxmauqYL9BXr0rkKs")]
    [TestCase("https://open.spotify.com/album/1R7vPDuTFeqCGOLj1JwfRH", LinkType.Album, "1R7vPDuTFeqCGOLj1JwfRH")]
    [TestCase("https://open.spotify.com/album/02LGNBgZPDm9PH9OMinhXt?si=k46URGstRDOpn1R-fZ6vug", LinkType.Album, "02LGNBgZPDm9PH9OMinhXt")]
    [TestCase("https://open.spotify.com/album/5rTy33plW3t6qW3i0ySBvG", LinkType.Album, "5rTy33plW3t6qW3i0ySBvG")]
    // artists, old format
    [TestCase("https://open.spotify.com/artist/6KJPsGYJN54GllYOKTleaj?si=6XuZ21vkTw66NiRuhi0m5A", LinkType.Artist, "6KJPsGYJN54GllYOKTleaj")]
    [TestCase("https://open.spotify.com/artist/7MqnCTCAX6SsIYYdJCQj9B", LinkType.Artist, "7MqnCTCAX6SsIYYdJCQj9B")]
    [TestCase("https://open.spotify.com/artist/630wzNP2OL7fl4Xl0GnMWq?si=Mng8vf73RFi-WvwfQUwXXQ", LinkType.Artist, "630wzNP2OL7fl4Xl0GnMWq")]
    [TestCase("https://open.spotify.com/artist/3dz0NnIZhtKKeXZxLOxCam", LinkType.Artist, "3dz0NnIZhtKKeXZxLOxCam")]
    [TestCase("https://open.spotify.com/artist/3Ri4H12KFyu98LMjSoij5V?si=raJBGkzfS_K_w-Kv8xVdiA", LinkType.Artist, "3Ri4H12KFyu98LMjSoij5V")]
    public async Task TestRecognizer(string link, LinkType expectedLinkType, string expectedId)
    {
        var resourceLink = await spotifyLinksRecognizeService.TryRecognizeAsync(link);
        resourceLink.Should().NotBeNull();
        resourceLink!.Type.Should().Be(expectedLinkType);
        resourceLink.Id.Should().BeEquivalentTo(expectedId);
    }
    
    private ISpotifyLinksRecognizeService spotifyLinksRecognizeService = null!;
}