using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WeatherForecast.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {


 //       services.AddSingleton<ICacheService, CacheService>();

       
        AddRedisCache(services, configuration);

        return services;
    }
    
    private static void AddRedisCache(IServiceCollection services, IConfiguration configuration)
    {
        // services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(
        //     configuration.GetSection("Redis:ConnectionString").Value!));
    }
}