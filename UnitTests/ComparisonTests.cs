using Core.StringComparison;
using Core.StringComparison.Extensions;
using MusicSearch.Dto.Models;

namespace UnitTests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
        comparison = new LevenshteinDistanceStringComparison();
    }

    [Test]
    public void Test_CompareTwoTracks()
    {
        var originalTrack = new TrackDto
        { 
            Artist = new ArtistDto
            {
                Name = "Linkin Park"
            },
            Title = "Guilty All the Same (feat. Rakim)",
            Album = new AlbumDto
            {
                Name = "The Hunting Party"
            }
        };
        
        var foundTrack1 = new TrackDto
        { 
            Artist = new ArtistDto
            {
                Name = "Linkin Park"
            },
            Title = "Guilty All the Same (feat. Rakim)",
            Album = new AlbumDto
            {
                Name = "The Hunting Party"
            }
        };
        
        var foundTrack2 = new TrackDto
        { 
            Artist = new ArtistDto
            {
                Name = "Linkin Park"
            },
            Title = "Guilty All the Same (feat. Rakim) - Acapella",
            Album = new AlbumDto
            {
                Name = "The Hunting Party"
            }
        };
        
        Assert.That(comparison.CompareTracks(originalTrack, foundTrack1), Is.GreaterThan(comparison.CompareTracks(originalTrack, foundTrack2)));
    }

    private IStringComparison comparison = null!;
}