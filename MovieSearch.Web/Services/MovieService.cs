using MovieSearch.Web.Contracts;
using MovieSearch.Web.Models;

namespace MovieSearch.Web.Services
{
    public class MovieService(IMovieClient movieClient, ISearchHistoryService searchHistoryService) : IMovieService
    {
        public async Task<MovieSearchResponse> SearchMoviesByTitleAsync(string title, CancellationToken cancellationToken = default)
        {
            var result = await movieClient.SearchMoviesByTitleAsync(title, cancellationToken);
            await searchHistoryService.AddOrUpdateSearchAsync(title, cancellationToken);
            return result;
        }

        public Task<MovieDetails?> GetMovieDetailsAsync(string imdbId, CancellationToken cancellationToken = default)
        {
           return movieClient.GetMovieDetailsAsync(imdbId, cancellationToken);
        }
    }
}
