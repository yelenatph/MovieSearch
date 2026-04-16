using System.Text.Json.Serialization;

namespace MovieSearch.Web.Models;

public class MovieSearchResponse
{
    public List<Movie> Search { get; set; } = [];

    [JsonPropertyName("totalResults")]
    public string? TotalResults { get; set; }

    public string? Error { get; set; }
}
