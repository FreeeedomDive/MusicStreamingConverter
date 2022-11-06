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
    public void Test1()
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
            Title = "Guilty All The Same",
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
            Title = "Guilty All the Same  [Acapella]",
            Album = new AlbumDto
            {
                Name = "The Hunting Party"
            }
        };
        
        Console.WriteLine(comparison.CompareTracks(originalTrack, foundTrack1)); // 79
        Console.WriteLine(comparison.CompareTracks(originalTrack, foundTrack2)); // 82
    }

    private IStringComparison comparison = null!;
}