namespace MovieSearch.Web.Contracts;

public interface ISearchHistoryRepository
{
    Task AddOrUpdateSearchAsync(string title, CancellationToken cancellationToken = default);

    Task<List<string>> GetLatestSearchesAsync(CancellationToken cancellationToken = default);
}
