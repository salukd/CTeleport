using WeatherForecast.Infrastructure.Services.Cache;

namespace WeatherForecast.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IWeatherForecastService, WeatherForecastService>();

        AddOpenWeatherApi(services, configuration);

        AddRedisCache(services, configuration);

        return services;
    }

    private static void AddOpenWeatherApi(IServiceCollection services, IConfiguration configuration)
    {
        var openWeatherMapSettings = configuration.GetSection("OpenWeatherMapApi")
                                         .Get<OpenWeatherMapSettings>() ??
                                     throw new InvalidOperationException("OpenWeatherMapApi configuration is missing.");

        services.AddTransient<ApiKeyMessageHandler>(
            _ => new ApiKeyMessageHandler(openWeatherMapSettings.ApiKey));
        services.AddRestEaseClient<IOpenWeatherApi>()
            .ConfigureHttpClient(c => { c.BaseAddress = new Uri(openWeatherMapSettings.Address); })
            .AddHttpMessageHandler<ApiKeyMessageHandler>()
            .AddPolicyHandler(PolicyFactory<OpenWeatherMapSettings>.GetTimeoutAndRetryPolicy(openWeatherMapSettings))
            .AddPolicyHandler(PolicyFactory<OpenWeatherMapSettings>.GetRateLimitPolicy(openWeatherMapSettings))
            .AddPolicyHandler(PolicyFactory<OpenWeatherMapSettings>.GetCircuitBreakerPolicy(openWeatherMapSettings));
    }

    private static void AddRedisCache(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ICacheService, CacheService>();
        services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(
            configuration.GetConnectionString("Cache")!));
    }
}