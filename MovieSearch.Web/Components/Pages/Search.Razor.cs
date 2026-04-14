using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;
using MovieSearch.Web.Contracts;
using MovieSearch.Web.Models;

namespace MovieSearch.Web.Components.Pages;

public partial class Search
{
    private string SearchQuery { get; set; } = string.Empty;

    private bool IsLoading { get; set; }

    private string? ErrorMessage { get; set; }

    private IReadOnlyList<string> LatestSearches { get; set; } = [];

    private IReadOnlyList<Movie> Movies { get; set; } = [];

    private PaginationState pagination = new() { ItemsPerPage = 10 };

    private readonly IMovieService _movieService;
    private readonly ISearchHistoryRepository _searchHistoryRepository;
    private readonly NavigationManager _navigationManager;


    public Search(IMovieService movieService, ISearchHistoryRepository searchHistoryRepository, NavigationManager navigationManager)
    {
        _movieService = movieService;
        _searchHistoryRepository = searchHistoryRepository;
        _navigationManager = navigationManager;
    }

    protected override async Task OnInitializedAsync()
    {
        await GetSearchHistory();
    }

    private async Task HandleSearch()
    {
        IsLoading = true;
        ErrorMessage = null;

        var _searchResponse = await _movieService.SearchMoviesByTitleAsync(SearchQuery);
        if (_searchResponse != null && _searchResponse.Error == null)
        {
            Movies = _searchResponse.Search;
            await pagination.SetCurrentPageIndexAsync(0);
        }

        if (_searchResponse?.Error != null)
        {
            ErrorMessage = _searchResponse.Error;
        }

        await GetSearchHistory();
        IsLoading = false;
    }

    private void OpenDetails(string imdbId) => _navigationManager.NavigateTo($"/movie/{imdbId}");

    private async Task SearchFromHistory(string query)
    {
        SearchQuery = query;
        await HandleSearch();
    }

    private async Task GetSearchHistory()
    {
        LatestSearches = await _searchHistoryRepository.GetLatestSearchesAsync();
    }
}
