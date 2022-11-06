namespace Core.StringComparison;

public class LevenshteinDistanceStringComparison : IStringComparison
{
    public int Compare(string original, string secondString)
    {
        var s1 = original.ToLower();
        var s2 = secondString.ToLower();

        if (s1.Length == 0)
        {
            return 0;
        }

        if (s2.Length == 0)
        {
            return 0;
        }

        var matrix = LevenshteinDistanceMatrix(s1, s2);
        var differenceAtOriginalStringLength = matrix[s1.Length, Math.Min(s1.Length, s2.Length)];

        return (int)(100 - (100d * differenceAtOriginalStringLength / s1.Length));
    }
    
    private static int[,] LevenshteinDistanceMatrix(string string1, string string2)
    {
        var s1Length = string1.Length;
        var s2Length = string2.Length;
        var distanceMatrix = new int[s1Length + 1, s2Length + 1];

        for (var i = 0; i <= s1Length; i++)
        {
            distanceMatrix[i, 0] = i;
        }

        for (var i = 0; i <= s2Length; i++)
        {
            distanceMatrix[0, i] = i;
        }

        for (var i = 1; i <= s1Length; i++)
        {
            for (var j = 1; j <= s2Length; j++)
            {
                var cost = (string2[j - 1] == string1[i - 1]) ? 0 : 1;

                distanceMatrix[i, j] = Math.Min(
                    Math.Min(distanceMatrix[i - 1, j] + 1, distanceMatrix[i, j - 1] + 1),
                    distanceMatrix[i - 1, j - 1] + cost);
            }
        }

        return distanceMatrix;
    }
}