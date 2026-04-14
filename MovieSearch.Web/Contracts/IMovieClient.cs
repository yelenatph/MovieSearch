using MovieSearch.Web.Models;

namespace MovieSearch.Web.Contracts;

public interface IMovieClient
{
    Task<MovieSearchResponse?> SearchMoviesByTitleAsync(string title, CancellationToken cancellationToken = default);

    Task<MovieDetails?> GetMovieDetailsAsync(string imdbId, CancellationToken cancellationToken = default);
}
