using Microsoft.Extensions.Options;
using MovieSearch.Web.Contracts;
using MovieSearch.Web.Services;

namespace MovieSearch.Web.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IMovieService, MovieService>();

        services.Configure<OmdbApiServiceSettings>(configuration.GetSection("OmdbApiServiceSettings"));

        services.AddHttpClient<IMovieClient, MovieClient>((sp, client) =>
        {
            var serviceSettings = configuration.GetSection(nameof(OmdbApiServiceSettings)).Get<OmdbApiServiceSettings>();
            client.BaseAddress = new Uri(serviceSettings?.BaseUrl ?? "https://www.omdbapi.com/");
        });

        return services;
    }
}
