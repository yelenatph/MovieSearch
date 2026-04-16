using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using MovieSearch.Web.Contracts;
using MovieSearch.Web.Models;
using MovieSearch.Web.Services;

namespace MovieSearch.Tests;

[TestFixture]
public class MovieServiceTests
{
    private IFixture _fixture;
    private Mock<IMovieClient> _movieClientMock = null!;
    private Mock<ISearchHistoryRepository> _searchHistoryRepositoryMock = null!;

    private IMovieService _movieService = null!;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _movieClientMock = _fixture.Freeze<Mock<IMovieClient>>();
        _searchHistoryRepositoryMock = _fixture.Freeze<Mock<ISearchHistoryRepository>>();

        _movieService = _fixture.Create<MovieService>();
    }

    [Test]
    public async Task SearchMoviesByTitleAsync_EmptyTitle_ReturnsErrorMessage()
    {
        // Arrange
        var title = string.Empty;
        string expectedErrorMessage = "Title is not provided";

        // Act
        var response = await _movieService.SearchMoviesByTitleAsync(title);

        // Assert
        Assert.That(response, Is.Not.Null);
        Assert.That(response!.Error, Is.EqualTo(expectedErrorMessage));
    }

    [Test]
    public async Task SearchMoviesByTitleAsync_TitleIsNull_ReturnsErrorMessage()
    {
        // Arrange
        string? title = null;
        string expectedErrorMessage = "Title is not provided";

        // Act
        var response = await _movieService.SearchMoviesByTitleAsync(title!);

        // Assert
        Assert.That(response, Is.Not.Null);
        Assert.That(response!.Error, Is.EqualTo(expectedErrorMessage));
    }

    [Test]
    public async Task SearchMoviesByTitleAsync_ValidTitle_ReturnsMoviesAndUpdatesHistory()
    {
        // Arrange
        var title = "Titanic";

        var expectedResponse = new MovieSearchResponse
        {
            Search = [new Movie { ImdbId = "tt0120338", Title = "Titanic", Year = "1997", Type = "movie" }]
        };

        _movieClientMock.Setup(client => client.SearchMoviesByTitleAsync(It.Is<string>(t => t == title), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var response = await _movieService.SearchMoviesByTitleAsync(title);

        // Assert
        Assert.That(response, Is.EqualTo(expectedResponse));
        _searchHistoryRepositoryMock.Verify(repo => repo.AddOrUpdateSearchAsync(It.Is<string>(t => t == title), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task SearchMoviesByTitleAsync_ClientThrowsException_ReturnsErrorMessage()
    {
        // Arrange
        var title = "Titanic";

        _movieClientMock.Setup(client => client.SearchMoviesByTitleAsync(It.Is<string>(t => t == title), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("boom"));

        // Act
        var response = await _movieService.SearchMoviesByTitleAsync(title);

        // Assert
        Assert.That(response, Is.Not.Null);
        Assert.That(response!.Error, Is.EqualTo("An error occurred while searching for movies: boom"));
    }

    [Test]
    public async Task GetMovieDetailsAsync_ImdbIdIsNull_ReturnsErrorMessage()
            {
        // Arrange
        string? imdbId = null;
        string expectedErrorMessage = "IMDB ID is not provided";

        // Act
        var response = await _movieService.GetMovieDetailsAsync(imdbId!);

        // Assert
        Assert.That(response, Is.Not.Null);
        Assert.That(response!.Error, Is.EqualTo(expectedErrorMessage));
    }

    [Test]
    public async Task GetMovieDetailsAsync_ImdbIdIsEmpty_ReturnsErrorMessage()
    {
        // Arrange
        string imdbId = string.Empty;
        string expectedErrorMessage = "IMDB ID is not provided";

        // Act
        var response = await _movieService.GetMovieDetailsAsync(imdbId);

        // Assert
        Assert.That(response, Is.Not.Null);
        Assert.That(response!.Error, Is.EqualTo(expectedErrorMessage));
    }

    [Test]
    public async Task GetMovieDetailsAsync_ImdbIdIsValid_ReturnsMovieDetails()
    {
        // Arrange
        var imdbId = "tt0120338";
        var expectedMovieDetails = new MovieDetails { ImdbId = imdbId, Title = "Titanic" };

        _movieClientMock.Setup(client => client.GetMovieDetailsAsync(It.Is<string>(id => id == imdbId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedMovieDetails);

        // Act
        var response = await _movieService.GetMovieDetailsAsync(imdbId);

        // Assert
        Assert.That(response, Is.EqualTo(expectedMovieDetails));
    }

    [Test]
    public async Task GetMovieDetailsAsync_ClientThrowsException_ReturnsErrorMessage()
    {
        // Arrange
        var imdbId = "tt0120338";

        _movieClientMock.Setup(client => client.GetMovieDetailsAsync(It.Is<string>(id => id == imdbId), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("boom"));

        // Act
        var response = await _movieService.GetMovieDetailsAsync(imdbId);

        // Assert
        Assert.That(response, Is.Not.Null);
        Assert.That(response!.Error, Is.EqualTo("An error occurred while retrieving movie details: boom"));
    }
}
