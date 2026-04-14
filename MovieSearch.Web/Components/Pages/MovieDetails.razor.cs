using Microsoft.AspNetCore.Components;
using MovieSearch.Web.Contracts;
using Models = MovieSearch.Web.Models;

namespace MovieSearch.Web.Components.Pages;

public partial class MovieDetails
{
    [Parameter]
    public string ImdbId { get; set; } = string.Empty;

    private Models.MovieDetails? Movie { get; set; }

    private readonly IMovieService _movieService;
    private readonly NavigationManager _navigationManager;

    public MovieDetails(IMovieService movieService, NavigationManager navigationManager)
    {
        _movieService = movieService;
        _navigationManager = navigationManager;
    }

    protected override async Task OnInitializedAsync()
    {
        Movie = await _movieService.GetMovieDetailsAsync(ImdbId);
    }

    private void BackToSearch() => _navigationManager.NavigateTo("/");
}

