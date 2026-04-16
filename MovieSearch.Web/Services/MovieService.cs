using MovieSearch.Web.Contracts;
using MovieSearch.Web.Models;

namespace MovieSearch.Web.Services
{
    public class MovieService(IMovieClient movieClient, ISearchHistoryRepository searchHistoryRepository) : IMovieService
    {
        public async Task<MovieSearchResponse?> SearchMoviesByTitleAsync(string title, int pageNumber = 1, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return new MovieSearchResponse { Error = "Title is not provided" };
            }

            try
            {
                var searchResult = await movieClient.SearchMoviesByTitleAsync(title.Trim(), pageNumber, cancellationToken);
                await searchHistoryRepository.AddOrUpdateSearchAsync(title, cancellationToken);
                return searchResult;

            }
            catch (Exception ex)
            {
                return new MovieSearchResponse { Error = $"An error occurred while searching for movies: {ex.Message}" };
            }
        }

        public async Task<MovieDetails?> GetMovieDetailsAsync(string imdbId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(imdbId))
            {
                return new MovieDetails { Error = "IMDB ID is not provided" };
            }

            try
            {
                return await movieClient.GetMovieDetailsAsync(imdbId, cancellationToken);

            }
            catch (Exception ex)
            {
                return new MovieDetails { Error = $"An error occurred while retrieving movie details: {ex.Message}"};
            }          
        }
    }
}
