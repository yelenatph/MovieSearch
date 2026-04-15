using Microsoft.EntityFrameworkCore;
using MovieSearch.Web.Models.Domain;

namespace MovieSearch.Web.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<SearchHistoryItem> SearchHistory { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SearchHistoryItem>(item =>
        {
            item.HasKey(i => i.Id);
            item.Property(i => i.Query).IsRequired();
            item.Property(i => i.LastSearchedAt).IsRequired();
        });
    }
}
