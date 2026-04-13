namespace MovieSearch.Web.Models;

public class SearchHistoryItem
{
    public int Id { get; set; }
    public required string Query { get; set; }
    public DateTime LastSearchedAt { get; set; }
}
