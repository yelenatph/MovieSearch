namespace MovieSearch.Web.Models;

public class MovieSearchResponse
{
    public List<Movie> Search { get; set; } = [];
    public bool Response { get; set; }
    public string? Error { get; set; }
}
