using Microsoft.Extensions.Options;
using MovieSearch.Web.Contracts;
using MovieSearch.Web.Infrastructure;
using MovieSearch.Web.Models;

namespace MovieSearch.Web.Services
{
    public class MovieService(IMovieClient movieClient) : IMovieService
    {
        public async Task<MovieSearchResponse> SearchMoviesByTitleAsync(string title, CancellationToken cancellationToken = default)
        {
            return await movieClient.SearchMoviesByTitleAsync(title, cancellationToken);
        }

        public Task<MovieDetails?> GetMovieDetailsAsync(string imdbId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
