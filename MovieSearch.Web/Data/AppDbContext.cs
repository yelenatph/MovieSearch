using Microsoft.EntityFrameworkCore;
using MovieSearch.Web.Models;

namespace MovieSearch.Web.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<SearchHistoryItem> SearchHistory { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SearchHistoryItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Query).IsRequired();
            entity.Property(e => e.LastSearchedAt).IsRequired();
        });
    }
}
