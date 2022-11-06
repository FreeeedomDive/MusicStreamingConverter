namespace Core.StringComparison.Extensions;

public static class StringExtensions
{
    public static Dictionary<char, int> ToSymbolsDictionary(this string input)
    {
        var result = new Dictionary<char, int>();
        foreach (var symbol in input.ToCharArray())
        {
            result[symbol] = result.ContainsKey(symbol) ? result[symbol] + 1 : 1;
        }

        return result;
    }

    public static int GetMatchingSymbolsCount(this string original, string secondString)
    {
        var originalStringSymbols = original.ToSymbolsDictionary();
        var secondsStringSymbols = secondString.ToSymbolsDictionary();
        return originalStringSymbols
            .Keys
            .Sum(originalStringSymbol =>
                Math.Min(originalStringSymbols[originalStringSymbol],
                    secondsStringSymbols.TryGetValue(originalStringSymbol, out var count) ? count : 0)
            );
    }
}