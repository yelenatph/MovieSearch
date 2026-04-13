using Microsoft.AspNetCore.Components;
using MovieSearch.Web.Contracts;
using MovieSearch.Web.Models;

namespace MovieSearch.Web.Components.Pages;

public partial class Home
{
    public string SearchQuery { get; set; } = string.Empty;

    private bool IsLoading { get; set; }

    private string? ErrorMessage { get; set; }

    private MovieSearchResponse? SearchResponse { get; set; }

    private List<Movie> Movies;

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    private readonly IMovieService _movieService;

    public Home(IMovieService movieService)
    {
        _movieService = movieService;
    }

    protected override async Task OnInitializedAsync()
    {
    }

    private async Task HandleSearch()
    {
        IsLoading = true;
        ErrorMessage = null;

        SearchResponse = await _movieService.SearchMoviesByTitleAsync(SearchQuery);

        if (SearchResponse.Error == null && SearchResponse.Search.Count > 0)
        {
           Movies = SearchResponse.Search;
        }
        else
        {
            ErrorMessage = SearchResponse.Error;
        }

        IsLoading = false;
    }

    private void OpenDetails(string imdbId) => NavigationManager.NavigateTo($"/movie/{imdbId}");
}
