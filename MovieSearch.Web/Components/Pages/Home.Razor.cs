using Microsoft.AspNetCore.Components;
using MovieSearch.Web.Contracts;
using MovieSearch.Web.Models;

namespace MovieSearch.Web.Components.Pages;

public partial class Home
{
    private string SearchQuery { get; set; } = string.Empty;

    private bool IsLoading { get; set; }

    private string? ErrorMessage { get; set; }

    private IReadOnlyList<string> LatestSearches { get; set; } = [];

    private IReadOnlyList<Movie> Movies { get; set; } = [];

    private readonly IMovieService _movieService;
    private readonly ISearchHistoryService _searchHistoryService;
    private readonly NavigationManager _navigationManager;


    public Home(IMovieService movieService, ISearchHistoryService searchHistoryService, NavigationManager navigationManager)
    {
        _movieService = movieService;
        _searchHistoryService = searchHistoryService;
        _navigationManager = navigationManager;
    }

    protected override async Task OnInitializedAsync()
    {
        LatestSearches = await _searchHistoryService.GetLatestSearchesAsync();
    }

    private async Task HandleSearch()
    {
        IsLoading = true;
        ErrorMessage = null;

        var _searchResponse = await _movieService.SearchMoviesByTitleAsync(SearchQuery);

        if (_searchResponse.Error == null && _searchResponse.Search.Count > 0)
        {
           Movies = _searchResponse.Search;
        }
        else
        {
            ErrorMessage = _searchResponse.Error;
        }

        IsLoading = false;
    }

    private void OpenDetails(string imdbId) => _navigationManager.NavigateTo($"/movie/{imdbId}");

    private async Task SearchFromHistory(string query)
    {
        SearchQuery = query;
        await HandleSearch();
    }
}
