﻿namespace TelegramBot.Core.Extensions;

public static class IntExtensions
{
    public static string PluralizeString(this int count, string singleForm, string severalForm, string manyForm)
    {
        var correctCount = count % 100;
        if (correctCount is >= 10 and <= 20
            || correctCount % 10 >= 5 && correctCount % 10 <= 9
            || correctCount % 10 == 0)
        {
            return $"{count} {manyForm}";
        }

        return correctCount % 10 == 1 ? $"{count} {singleForm}" : $"{count} {severalForm}";
    }
}