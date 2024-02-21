namespace TelegramBot.Core.Models;

public class ResourceCompareResult<T>
{
    public T Resource { get; set; }
    public int Confidence { get; set; }
}