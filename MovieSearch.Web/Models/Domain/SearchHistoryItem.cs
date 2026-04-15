namespace MovieSearch.Web.Models.Domain;

public class SearchHistoryItem
{
    public int Id { get; set; }

    public required string Query { get; set; }

    public DateTime LastSearchedAt { get; set; }
}
