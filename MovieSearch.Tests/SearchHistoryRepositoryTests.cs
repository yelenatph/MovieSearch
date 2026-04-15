using Microsoft.EntityFrameworkCore;
using MovieSearch.Web.Data;
using MovieSearch.Web.Data.Repositories;

namespace MovieSearch.Tests;

[TestFixture]
public class SearchHistoryRepositoryTests
{
    [Test]
    public async Task AddOrUpdateSearchAsync_SearchQueryDoesNotExists_AddsSearchQueryToDB()
    {
        // Arrange
        var dbContext = CreateDbContext();
        var repository = new SearchHistoryRepository(dbContext);
        var searchQuery = "Titanic";

        // Act
        await repository.AddOrUpdateSearchAsync(searchQuery);

        // Assert
        var searchHistory = await dbContext.SearchHistory.ToListAsync();

        Assert.That(searchHistory.Count, Is.EqualTo(1));
        Assert.That(searchHistory[0].Query, Is.EqualTo(searchQuery));
    }

    [Test]
    public async Task AddOrUpdateSearchAsync_SearchQueryExists_UpdatesExistingSearch()
    {
        // Arrange
        var dbContext = CreateDbContext();
        var repository = new SearchHistoryRepository(dbContext);
        var searchQuery = "Titanic";

        // Act
        await repository.AddOrUpdateSearchAsync(searchQuery);
        await Task.Delay(10);
        await repository.AddOrUpdateSearchAsync(searchQuery);

        // Assert
        var searchHistory = await dbContext.SearchHistory.ToListAsync();

        Assert.That(searchHistory.Count, Is.EqualTo(1));
        Assert.That(searchHistory[0].Query, Is.EqualTo(searchQuery));
    }

    [Test]
    public async Task AddOrUpdateSearchAsync_SearchQueryExistsWithDifferentCasing_UpdatesExistingSearch()
    {
        // Arrange
        var dbContext = CreateDbContext();
        var repository = new SearchHistoryRepository(dbContext);

        // Act
        await repository.AddOrUpdateSearchAsync("Titanic");
        await repository.AddOrUpdateSearchAsync("titanic");

        // Assert
        var searchHistory = await dbContext.SearchHistory.ToListAsync();
        Assert.That(searchHistory.Count, Is.EqualTo(1));
    }

    [Test]
    public async Task AddOrUpdateSearchAsync_KeepsOnlyFiveLastQueries()
    {
        // Arrange
        var dbContext = CreateDbContext();
        var repository = new SearchHistoryRepository(dbContext);
        var searchQuery = "Titanic";

        var titles = new List<string> { "Inception", "The Matrix", "Interstellar", "The Godfather", "Pulp Fiction" };

        foreach (var title in titles)
        {
            await repository.AddOrUpdateSearchAsync(title);
            await Task.Delay(10);
        }

        // Act
        await repository.AddOrUpdateSearchAsync(searchQuery);

        // Assert
        var searchHistory = await dbContext.SearchHistory.OrderByDescending(x => x.LastSearchedAt).ToListAsync();

        Assert.That(searchHistory.Count, Is.EqualTo(5));
        Assert.That(searchHistory[0].Query, Is.EqualTo(searchQuery));
    }

    [Test]
    public async Task GetLatestSearchesAsync_ReturnsFiveLatestSearches()
    {
        // Arrange
        var dbContext = CreateDbContext();
        var repository = new SearchHistoryRepository(dbContext);
        var searchQuery = "Titanic";

        var titles = new List<string> { "Inception", "The Matrix", "Interstellar", "The Godfather", "Pulp Fiction" };

        foreach (var title in titles)
        {
            await repository.AddOrUpdateSearchAsync(title);
        }

        // Act
        await repository.AddOrUpdateSearchAsync(searchQuery);

        // Assert
        var latestSearches = await repository.GetLatestSearchesAsync();

        Assert.That(latestSearches.Count, Is.EqualTo(5));
        Assert.That(latestSearches[0], Is.EqualTo(searchQuery));
        Assert.That(latestSearches[1], Is.EqualTo("Pulp Fiction"));
        Assert.That(latestSearches[2], Is.EqualTo("The Godfather"));
        Assert.That(latestSearches[3], Is.EqualTo("Interstellar"));
        Assert.That(latestSearches[4], Is.EqualTo("The Matrix"));
    }

    private static AppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }
}


