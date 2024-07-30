using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSubstitute;
using StackExchange.Redis;
using WeatherForecast.Infrastructure.ExternalApis.OpenWeather;

namespace WeatherForecast.Api.Rest.IntegrationTests.Controllers;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.AddSingleton(Substitute.For<IOpenWeatherApi>());
            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("localhost"));
        });

        return base.CreateHost(builder);
    }
}