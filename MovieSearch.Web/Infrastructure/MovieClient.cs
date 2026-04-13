using Microsoft.Extensions.Options;
using MovieSearch.Web.Contracts;
using MovieSearch.Web.Models;

namespace MovieSearch.Web.Infrastructure;

public class MovieClient(HttpClient httpClient, IOptions<OmdbApiServiceSettings> settings) : IMovieClient
{
    private readonly OmdbApiServiceSettings _settings = settings.Value;

    public async Task<MovieSearchResponse> SearchMoviesByTitleAsync(string title, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            return new MovieSearchResponse { Error = "Title is not provided" };
        }

        var url = $"?apikey={_settings.ApiKey}&s={Uri.EscapeDataString(title.Trim())}";

        MovieSearchResponse? response;
        try
        {
            response = await httpClient.GetFromJsonAsync<MovieSearchResponse>(url, cancellationToken);
        }
        catch (Exception ex)
        {
            return new MovieSearchResponse { Error = ex.Message };
        }

        if (response == null)
        {
            return new MovieSearchResponse { Error = "Movie not found" };
        }

        return response;
    }

    public Task<MovieDetails?> GetMovieDetailsAsync(string imdbId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
