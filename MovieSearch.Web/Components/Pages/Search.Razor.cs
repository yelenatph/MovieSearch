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

    private IReadOnlyList<Movie> Movies { get; set; } = [];

    private IQueryable<Movie> MoviesQueryable => Movies.AsQueryable();

    private int CurrentPage => _pageNumber;

    private int TotalPages => Math.Max(1, (int)Math.Ceiling(_totalMovieCount / (double)PageSize));

    private bool CanGoToPreviousPage => CurrentPage > 1;

    private bool CanGoToNextPage => CurrentPage < TotalPages;

    private int _pageNumber;

    private int _totalMovieCount;

    private readonly IMovieService _movieService;
    private readonly ISearchHistoryRepository _searchHistoryRepository;

    public Search(IMovieService movieService, ISearchHistoryRepository searchHistoryRepository)
    {
        _movieService = movieService;
        _searchHistoryRepository = searchHistoryRepository;
    }

    protected override async Task OnInitializedAsync()
    {
        await GetSearchHistory();
    }

    private async Task HandleSearch()
    {
        HasSearched = true;
        ErrorMessage = null;
        _totalMovieCount = 0;
        _pageNumber = 1;
        Movies = [];

        await LoadMovieAsync();
        await GetSearchHistory();
    }

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

        _pageNumber--;
        await LoadMovieAsync();
    }

    private async Task GoToNextPageAsync()
    {
        if (!CanGoToNextPage)
        {
            return;
        }

        _pageNumber++;
        await LoadMovieAsync();
    }

    private async Task LoadMovieAsync()
    {
        if (!HasSearched || string.IsNullOrWhiteSpace(SearchQuery))
        {
            return;
        }

        IsLoading = true;
        try
        {
            var searchResponse = await _movieService.SearchMoviesByTitleAsync(SearchQuery, _pageNumber);

            if (searchResponse is null)
            {
                ErrorMessage = "An unexpected error occurred while searching for movies.";
                _totalMovieCount = 0;
                Movies = [];
                return;
            }

            if (!string.IsNullOrWhiteSpace(searchResponse.Error))
            {
                ErrorMessage = searchResponse.Error;
                _totalMovieCount = 0;
                Movies = [];
                return;
            }

            ErrorMessage = null;
            _totalMovieCount = int.TryParse(searchResponse.TotalResults, out var total)
                ? total
                : searchResponse.Search.Count;
            Movies = searchResponse.Search;
        }
        finally
        {
            IsLoading = false;
        }
    }
}
