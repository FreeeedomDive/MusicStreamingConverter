namespace Core.StringComparison;

public class LevenshteinDistanceStringComparison : IStringComparison
{
    public int Compare(string original, string secondString)
    {
        return (int)((100d * LevenshteinDistance(original, secondString)) / (original.Length + secondString.Length));
    }
    
    private static int LevenshteinDistance(string text1, int len1, string text2, int len2)
    {
        if (len1 == 0)
        {
            return len2;
        }

        if (len2 == 0)
        {
            return len1;
        }
    
        var substitutionCost = 0;
        if(text1[len1 - 1] != text2[len2 - 1])
        {
            substitutionCost = 1;
        }

        var deletion = LevenshteinDistance(text1, len1 - 1, text2, len2) + 1;
        var insertion = LevenshteinDistance(text1, len1, text2, len2 - 1) + 1;
        var substitution = LevenshteinDistance(text1, len1 - 1, text2, len2 - 1) + substitutionCost;

        return Min(deletion, insertion, substitution);
    }

    static int LevenshteinDistance(string word1, string word2) =>
        LevenshteinDistance(word1, word1.Length, word2, word2.Length);

    private static int Min(int a, int b, int c) => (a = a < b ? a : b) < c ? a : c;
}