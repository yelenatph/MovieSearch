using System.Text.Json.Serialization;

namespace MovieSearch.Web.Models;

public class Movie
{
    [JsonPropertyName("imdbID")]
    public string? ImdbId { get; init; }
    public string? Title { get; init; }
    public string? Year { get; init; }
    public string? Type { get; init; }

    [JsonPropertyName("Poster")]
    public string? PosterUrl { get; init; }
}
