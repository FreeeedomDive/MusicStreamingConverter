using Core.StringComparison.Extensions;

namespace Core.StringComparison;

public class JaccardIndexStringComparison : IStringComparison
{
    public int Compare(string original, string secondString)
    {
        var matchingSymbolsCount = original.GetMatchingSymbolsCount(secondString);

        return matchingSymbolsCount / (original.Length + secondString.Length - matchingSymbolsCount);
    }
}