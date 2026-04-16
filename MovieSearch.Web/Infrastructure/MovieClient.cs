using Microsoft.Extensions.Options;
using MovieSearch.Web.Contracts;
using MovieSearch.Web.Models;

namespace MovieSearch.Web.Infrastructure;

public class MovieClient(HttpClient httpClient, IOptions<OmdbApiServiceSettings> settings) : IMovieClient
{
    private readonly OmdbApiServiceSettings _settings = settings.Value;

    public async Task<MovieSearchResponse?> SearchMoviesByTitleAsync(string title, int pageNumber = 1, CancellationToken cancellationToken = default)
    {
        var url = $"?apikey={_settings.ApiKey}&s={Uri.EscapeDataString(title)}&page={pageNumber}";

        return  await httpClient.GetFromJsonAsync<MovieSearchResponse>(url, cancellationToken);
    }

    public async Task<MovieDetails?> GetMovieDetailsAsync(string imdbId, CancellationToken cancellationToken = default)
    {
        var url = $"?apikey={_settings.ApiKey}&i={Uri.EscapeDataString(imdbId)}";

        return  await httpClient.GetFromJsonAsync<MovieDetails>(url, cancellationToken);
    }
}
