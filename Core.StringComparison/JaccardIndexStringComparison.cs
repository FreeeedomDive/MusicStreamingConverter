﻿using Core.StringComparison.Extensions;

namespace Core.StringComparison;

public class JaccardIndexStringComparison : IStringComparison
{
    public int Compare(string original, string secondString)
    {
        var matchingSymbolsCount = original.ToLower().GetMatchingSymbolsCount(secondString.ToLower());

        return Math.Max(0, (int)(100d * matchingSymbolsCount / (original.Length + secondString.Length - matchingSymbolsCount)));
    }
}