namespace TelegramBot.Core.Models;

public class ResourceMatch<T>
{
    public T Original { get; set; }
    public T? MatchResult { get; set; }
    public int FoundResultsCount { get; set; }
    public int MatchConfidence { get; set; }
}