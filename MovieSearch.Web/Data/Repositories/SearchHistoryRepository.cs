using Microsoft.EntityFrameworkCore;
using MovieSearch.Web.Contracts;
using MovieSearch.Web.Models;

namespace MovieSearch.Web.Data.Repositories;

public class SearchHistoryRepository(AppDbContext dbContext) : ISearchHistoryRepository
{
    public async Task AddOrUpdateSearchAsync(string title, CancellationToken cancellationToken = default)
    {
        var existingSearch = await dbContext.SearchHistory.FirstOrDefaultAsync(s => s.Query.ToLower() == title.ToLower(), cancellationToken);
        if (existingSearch == null)
        {
            dbContext.SearchHistory.Add(new SearchHistoryItem
            {
                Query = title,
                LastSearchedAt = DateTime.UtcNow
            });
        }
        else
        {
            existingSearch.LastSearchedAt = DateTime.UtcNow;
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        var allSearches = await dbContext.SearchHistory.OrderByDescending(x => x.LastSearchedAt).ToListAsync(cancellationToken);
       
        if (allSearches.Count > 5)
        {
            var searchesToRemove = allSearches.Skip(5).ToList();
            dbContext.SearchHistory.RemoveRange(searchesToRemove);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<List<string>> GetLatestSearchesAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.SearchHistory.OrderByDescending(s => s.LastSearchedAt)
            .Select(s => s.Query)
            .ToListAsync(cancellationToken);
    }
}
