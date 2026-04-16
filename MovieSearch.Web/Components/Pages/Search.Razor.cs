using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;
using MovieSearch.Web.Contracts;
using MovieSearch.Web.Models;

namespace MovieSearch.Web.Components.Pages;

public partial class Search
{
    private const int PageSize = 10;

    private string SearchQuery { get; set; } = string.Empty;

    private bool IsLoading { get; set; }

    private string? ErrorMessage { get; set; }

    private bool HasSearched { get; set; }

    private IReadOnlyList<string> LatestSearches { get; set; } = [];

    private PaginationState pagination = new() { ItemsPerPage = PageSize };

    private GridItemsProvider<Movie> movieProvider = default!;

    private int TotalMovieCount;

    private int CurrentPage => pagination.CurrentPageIndex;

    private int TotalPages => Math.Max(1, (int)Math.Ceiling(TotalMovieCount / (double)PageSize));

    private bool CanGoToPreviousPage => CurrentPage > 0;

    private bool CanGoToNextPage => CurrentPage < TotalPages - 1;

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
        movieProvider = LoadMoviesAsync;
        await GetSearchHistory();
    }

    private async Task HandleSearch()
    {
        HasSearched = true;
        ErrorMessage = null;
        TotalMovieCount = 0;

        var searchResponse = await _movieService.SearchMoviesByTitleAsync(SearchQuery);
        if (searchResponse != null && searchResponse.Error == null)
        {
           TotalMovieCount = int.TryParse(searchResponse.TotalResults, out var total) ? total : searchResponse.Search.Count;
           await pagination.SetCurrentPageIndexAsync(0);
        }

        if (searchResponse?.Error != null)
        {
            ErrorMessage = searchResponse.Error;
        }

        await GetSearchHistory();
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

    private async Task GoToPreviousPageAsync()
    {
        if (!CanGoToPreviousPage)
        {
            return;
        }

        await pagination.SetCurrentPageIndexAsync(CurrentPage - 1);
    }

    private async Task GoToNextPageAsync()
    {
        if (!CanGoToNextPage)
        {
            return;
        }

        await pagination.SetCurrentPageIndexAsync(CurrentPage + 1);
    }

    private async ValueTask<GridItemsProviderResult<Movie>> LoadMoviesAsync(GridItemsProviderRequest<Movie> request)
    {
        if (!HasSearched || string.IsNullOrWhiteSpace(SearchQuery))
        {
            return GridItemsProviderResult.From(Array.Empty<Movie>(), 0);
        }

        IsLoading = true;
        try
        {
            var pageSize = request.Count ?? PageSize;
            var pageNumber = (request.StartIndex / pageSize) + 1;
            var searchResponse = await _movieService.SearchMoviesByTitleAsync(SearchQuery, pageNumber);

            if (searchResponse is null)
            {
                ErrorMessage = "An unexpected error occurred while searching for movies.";
                TotalMovieCount = 0;
                return GridItemsProviderResult.From(Array.Empty<Movie>(), 0);
            }

            if (!string.IsNullOrWhiteSpace(searchResponse.Error))
            {
                ErrorMessage = searchResponse.Error;
                TotalMovieCount = 0;
                return GridItemsProviderResult.From(Array.Empty<Movie>(), 0);
            }

            ErrorMessage = null;
            TotalMovieCount = int.TryParse(searchResponse.TotalResults, out var total) ? total : searchResponse.Search.Count;
            return GridItemsProviderResult.From(searchResponse.Search, TotalMovieCount);
        }
        finally
        {
            IsLoading = false;
        }
    }
}
