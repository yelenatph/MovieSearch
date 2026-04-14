using Microsoft.AspNetCore.Components;
using MovieSearch.Web.Contracts;
using Models = MovieSearch.Web.Models;

namespace MovieSearch.Web.Components.Pages;

public partial class MovieDetails
{
    [Parameter]
    public string ImdbId { get; set; } = string.Empty;

    private bool IsLoading { get; set; }

    private string? ErrorMessage { get; set; }

    private Models.MovieDetails? Movie { get; set; }

    private readonly IMovieService _movieService;
    private readonly NavigationManager _navigationManager;

    public MovieDetails(IMovieService movieService, NavigationManager navigationManager)
    {
        _movieService = movieService;
        _navigationManager = navigationManager;
    }

    protected override async Task OnParametersSetAsync()
    {
        IsLoading = true;
        ErrorMessage = null;

        Movie = await _movieService.GetMovieDetailsAsync(ImdbId);
        if (Movie is null)
        {
            ErrorMessage = "Movie details were not found.";
            IsLoading = false;
            return;
        }

        if (!string.IsNullOrWhiteSpace(Movie.Error))
        {
            ErrorMessage = Movie.Error;
            Movie = null;
        }

        IsLoading = false;
    }

    private void BackToSearch() => _navigationManager.NavigateTo("/");

    private bool HasPoster =>
        Movie?.PosterUrl is not null &&
        !string.Equals(Movie.PosterUrl, "N/A", StringComparison.OrdinalIgnoreCase);

    private string ValueOrDash(string? value) => string.IsNullOrWhiteSpace(value) ? "-" : value;
}

