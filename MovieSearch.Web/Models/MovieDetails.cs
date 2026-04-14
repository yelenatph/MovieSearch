using System.Text.Json.Serialization;

namespace MovieSearch.Web.Models;

public class MovieDetails : Movie
{
    public string? Rated { get; init; }
    public string? Released { get; init; }
    public string? Runtime { get; init; }
    public string? Genre { get; init; }
    public string? Director { get; init; }
    public string? Writer { get; init; }
    public string? Actors { get; init; }
    public string? Plot { get; init; }
    public string? Language { get; init; }
    public string? Country { get; init; }
    public string? Awards { get; init; }
    public List<Rating> Ratings { get; init; } = [];
    public string? Metascore { get; init; }
    [JsonPropertyName("imdbRating")]
    public string? ImdbRating { get; init; }
    [JsonPropertyName("imdbVotes")]
    public string? ImdbVotes { get; init; }
    public string? DVD { get; init; }
    public string? BoxOffice { get; init; }
    public string? Production { get; init; }
    public string? Website { get; init; }
    public string? Response { get; init; }
    public string? Error { get; init; }
}

public record Rating(string Source, string Value);
