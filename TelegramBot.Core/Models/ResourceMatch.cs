namespace TelegramBot.Core.Models;

public class ResourceMatch<T>
{
    public T? MatchResult { get; set; }
    public int FoundResults { get; set; }
    public int MatchConfidence { get; set; }
}