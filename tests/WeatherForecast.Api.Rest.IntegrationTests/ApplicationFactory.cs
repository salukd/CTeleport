using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using RestEase.HttpClientFactory;
using Testcontainers.Redis;
using WeatherForecast.Api.Rest.IntegrationTests.ApiMocks;
using WeatherForecast.Infrastructure.ExternalApis.OpenWeather;

namespace WeatherForecast.Api.Rest.IntegrationTests;

public class ApplicationFactory : WebApplicationFactory<IApiMarker>, IAsyncLifetime
{
    private readonly OpenWeatherMapApiServer _weatherMapApiServer = new();

    readonly RedisContainer _redisContainer = new RedisBuilder()
        .WithImage("redis:7.0")
        .Build();
    

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var currentEnv = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        builder.UseEnvironment(!string.IsNullOrWhiteSpace(currentEnv) ? currentEnv : "LocalIntegrationTests");
        
        builder.UseContentRoot(Directory.GetCurrentDirectory());
        
        builder.ConfigureLogging(logging =>
        {
            logging.ClearProviders();
        });
        
        builder.ConfigureServices(services =>
        {
            services.RemoveAll(typeof(IOpenWeatherApi));

            services.AddRestEaseClient<IOpenWeatherApi>(_ => { })
                .ConfigureHttpClient(client => client.BaseAddress = new Uri(_weatherMapApiServer.Url));
        });
        
        builder.ConfigureAppConfiguration((_, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string>
            {
                { "Redis:ConnectionString", _redisContainer.GetConnectionString() }
            }!);
        });
    }
    
    public async Task InitializeAsync()
    {
        _weatherMapApiServer.Start();
        _weatherMapApiServer.SetupSuccess();
        
        await _redisContainer.StartAsync();
    }

    public async Task DisposeAsync()
    {
        _weatherMapApiServer.Dispose();

        await _redisContainer.StopAsync();
    }
}

[CollectionDefinition("ApplicationFactory")]
public sealed class ApplicationFactoryCollection : ICollectionFixture<ApplicationFactory>
{
}