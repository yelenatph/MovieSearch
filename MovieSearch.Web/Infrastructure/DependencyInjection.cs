using Microsoft.EntityFrameworkCore;
using MovieSearch.Web.Contracts;
using MovieSearch.Web.Data;
using MovieSearch.Web.Data.Repositories;
using MovieSearch.Web.Services;

namespace MovieSearch.Web.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<OmdbApiServiceSettings>(configuration.GetSection("OmdbApiServiceSettings"));
        services.AddDbContext<AppDbContext>(options => options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IMovieService, MovieService>();
        services.AddScoped<ISearchHistoryRepository, SearchHistoryRepository>();

        services.AddHttpClient<IMovieClient, MovieClient>((sp, client) =>
        {
            var serviceSettings = configuration.GetSection(nameof(OmdbApiServiceSettings)).Get<OmdbApiServiceSettings>();
            client.BaseAddress = new Uri(serviceSettings?.BaseUrl ?? throw new ArgumentNullException("BaseUrl is not configured"));
        });

        return services;
    }
}
