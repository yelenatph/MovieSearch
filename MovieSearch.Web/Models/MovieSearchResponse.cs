namespace MovieSearch.Web.Models;

public class MovieSearchResponse
{
    public List<Movie> Search { get; set; } = [];

    public string? Error { get; set; }
}
