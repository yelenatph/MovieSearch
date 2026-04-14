using MovieSearch.Web.Contracts;

namespace MovieSearch.Web.Services;

public class SearchHistoryService : ISearchHistoryService
{
        private readonly ISearchHistoryRepository _searchHistoryRepository;
    
        public SearchHistoryService(ISearchHistoryRepository searchHistoryRepository)
        {
            _searchHistoryRepository = searchHistoryRepository;
        }
    
        public Task AddOrUpdateSearchAsync(string title, CancellationToken cancellationToken = default)
        {
            return _searchHistoryRepository.AddOrUpdateSearchAsync(title, cancellationToken);
        }
    
        public Task<List<string>> GetLatestSearchesAsync(CancellationToken cancellationToken = default)
        {
            return _searchHistoryRepository.GetLatestSearchesAsync(cancellationToken);
        }
}
